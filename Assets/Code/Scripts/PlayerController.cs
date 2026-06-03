using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : DefaultCharacter
{
    
    [Networked] public float spd { get; set; } = 4f;
    [Header("Movement")]
    private Vector2 move;

    [Header("Animator")]
    [SerializeField] Animator animator;
    [SerializeField] private NetworkMecanimAnimator networkAnimator;

    [Header("Attack")]
    [SerializeField] private Transform atkPoint;
    [SerializeField] private float atkRange;
    [SerializeField] private int atkDamage = 100;
    [SerializeField] private LayerMask enemyLayers;

    [Header("Health")]
    [SerializeField] private HealthBar healthBar;
    private float lastHealth;
    
    private CharacterController controller;

    private CameraFollow mainCamera;
    public bool IsLocalPlayer
    {
        get
        {
            if (Object == null || !Object.IsValid) return false;

            return Object.HasStateAuthority;
        }
    }

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        
        if (networkAnimator == null)
        {
            networkAnimator = GetComponent<NetworkMecanimAnimator>();
        }
    }

    public override void Spawned()
    {
        base.Spawned();

        controller.enabled = false;
        Vector3 safePos = transform.position;
        safePos.y = 0.5f;
        transform.position = safePos;
        controller.enabled = true;

        /*if (HasStateAuthority)
        {
            GameManager.Instance.Rpc_RegisterPlayer();
        }*/
        if (HasStateAuthority) 
        {
            StartCoroutine(WaitAndRegister());
        }
        if (IsLocalPlayer) 
        {
            mainCamera = FindObjectOfType<CameraFollow>();

            if (mainCamera != null) 
            {
                mainCamera.SetTarget(transform);
                //Debug.Log("CÂMERA SEQUESTRADA COM SUCESSO PELO JOGADOR LOCAL!");
            }
            else
            {
                //Debug.LogError("Câmera não encontrada na cena!");
            }
            
            healthBar = FindObjectOfType<HealthBar>();

            if (healthBar != null)
            {
                healthBar.SetMaxHealth(maxHealth);
            }
            else
            {
                //Debug.LogError("HealthBar não encontrada na cena!");
            }
        } 
        //Debug.Log("Player local criado e posicionado no Y=0.5");
    }
    private IEnumerator WaitAndRegister()
    {
        yield return new WaitUntil(() => 
            GameManager.Instance != null && 
            GameManager.Instance.Object != null && 
            GameManager.Instance.Object.IsValid
        );

        GameManager.Instance.Rpc_RegisterPlayer();
        //Debug.Log("Registro enviado ao GameManager com segurança!");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer) return;
        move = context.ReadValue<Vector2>();
    } 

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!IsLocalPlayer) return;
        
        if (context.started)
        {
            if (networkAnimator != null)
            {
                networkAnimator.SetTrigger("Attack");
            }
            else
            {
                animator.SetTrigger("Attack");
            }
            
            Attack();
        }
    }
    
    public override void FixedUpdateNetwork()
    {
        if (!IsLocalPlayer) return;
        MovePlayer();
    }

    public override void Render()
    {
        if (IsLocalPlayer)
        {
            if (lastHealth != CurrentHealth)
            {
                lastHealth = CurrentHealth;
                if (healthBar != null)
                {
                    healthBar.UpdateHealth(CurrentHealth);
                }
            }

            if (mainCamera == null)
            {
                mainCamera = FindObjectOfType<CameraFollow>();
                
                if (mainCamera != null)
                {
                    mainCamera.SetTarget(transform);
                    //Debug.Log($"[{gameObject.name}] SUCESSO! Câmera local encontrada e vinculada ao jogador.");
                }
                else
                {
                    //Debug.LogWarning($"[{gameObject.name}] SOCORRO! Nenhuma CameraFollow encontrada na cena do PC!");
                }
            }

            if (mainCamera != null)
            {
                mainCamera.UpdateCameraNetwork();
            }
        }
        
    }

    private void MovePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y).normalized;
        controller.Move(movement * Mathf.Clamp(spd, 1, 8) * Runner.DeltaTime);
    }

    private void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(atkPoint.position, atkRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();

            if (enemyController != null)
            {
                enemyController.Rpc_TakeDamage(atkDamage);
            }
        }
    }

    protected override void Death()
    {
        if (!HasStateAuthority) return;
        Debug.Log("Player morreu");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Rpc_PlayerDied();
        }

        if (Runner != null && Object != null && Object.IsValid)
        {
            PlayerRef player = Object.StateAuthority;
            Runner.SetPlayerObject(player, null);
            Runner.Despawn(Object);
        }
    }


    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_ApplySpeedBoost(float multiplier)
    {
        spd *= multiplier;
    }
    #region Debugs
    private void OnDrawGizmos()
    {
        if (atkPoint != null)
        {
            Gizmos.color = Color.red;  
            Gizmos.DrawWireSphere(atkPoint.position, atkRange);
        }
    }
    #endregion
}