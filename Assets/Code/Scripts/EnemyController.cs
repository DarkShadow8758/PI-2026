using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : DefaultCharacter
{
    [Header("Targeting")]
    public string playerTag = "Player";
    public float chaseRange = 15f;
    private NavMeshAgent agent;
    private Transform target;

    [Header("Effects")]
    [SerializeField] private GameObject FloatingTextPrefab;
    [SerializeField] private Transform textPoint;

    [Header("Attack")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 1f;

    [Header("Drop")]
    [SerializeField] private DropManager dropManager;
    [SerializeField] private Transform dropPoint;

    private EnemySpawner spawner;

    [Networked] private TickTimer attackCooldownTimer { get; set; }
    [Networked] private TickTimer targetUpdateTimer { get; set; }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
        if (agent == null)
        {
            Debug.LogError("HostileAI precisa de um NavMeshAgent no mesmo GameObject.");
        }    
    }

    public override void Spawned()
    {
        base.Spawned();
        if (agent != null)
        {
            agent.Warp(transform.position);
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;

        if (targetUpdateTimer.ExpiredOrNotRunning(Runner))
        {
            UpdateTarget();
            targetUpdateTimer = TickTimer.CreateFromSeconds(Runner, 0.5f);
        }

        if (target != null && Vector3.Distance(transform.position, target.position) <= chaseRange)
        {
            agent.SetDestination(target.position);
        }
    }

    private void UpdateTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);
        if (players.Length == 0)
        {
            target = null;
            return;
        }

        Transform nearest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject p in players)
        {
            float dist = Vector3.Distance(currentPos, p.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = p.transform;
            }
        }
        target = nearest;
    }

    public void SetSpawner(EnemySpawner _spawner)
    {
        spawner = _spawner;
    }

    protected override void ShowDamageVisuals(float damageAmount)
    {
        if (FloatingTextPrefab != null && textPoint != null)
        {
            GameObject go = Instantiate(FloatingTextPrefab, textPoint.position, Quaternion.identity);
            
            var tmp = go.GetComponent<TMPro.TextMeshPro>();
            if (tmp != null)
            {
                tmp.text = Mathf.RoundToInt(damageAmount).ToString();
                
                if (damageAmount > 20f)
                {
                    tmp.color = Color.yellow;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!HasStateAuthority) return;

        if (other.CompareTag("Player"))
        {
            if (attackCooldownTimer.ExpiredOrNotRunning(Runner))
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.Rpc_TakeDamage(damage);
                    attackCooldownTimer = TickTimer.CreateFromSeconds(Runner, attackCooldown);
                }
            }
        }
    }

    protected override void Death()
    {
        if (dropManager != null) dropManager.TryDrop(dropPoint.position, Runner);
        if (spawner != null) spawner.Rpc_ReportEnemyDeath();

        Runner.Despawn(Object);
    }
}