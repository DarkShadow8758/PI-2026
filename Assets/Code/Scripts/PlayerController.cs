using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : DefaultCharacter
{
    [Header("Movement")]
    [SerializeField] private float spd;
    private Vector2 move;
    [Header("Animator")]
    [SerializeField] Animator animator;
    [Header("Attack")]
    [SerializeField] private Transform atkPoint;
    [SerializeField] private float atkRange;
    [SerializeField] private int atkDamage = 100;
    [SerializeField] private LayerMask enemyLayers;
    

    private CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    } 

    public void OnAttack(InputAction.CallbackContext context)
    {
        //Debug.Log($"Botão pressionado! Fase: {context.phase}");
        if (context.started)
        {
            animator.SetTrigger("Attack");
            Debug.Log("Atacando");
            Attack();
        }
    }
    
    void Update()
    {
        MovePlayer();
    }

    public void MovePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y).normalized;

        controller.Move(movement * spd * Time.deltaTime);
    }

    public void Attack()
    {
        
        //Debug.Log("atacando");
        Collider[] hitEnemies = Physics.OverlapSphere(atkPoint.position, atkRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            //Debug.Log("Acertou " + enemy.name);
            var (damage, isCritical) = GetDamage(atkDamage);
            enemy.GetComponent<EnemyController>().TakeDamage(damage, isCritical);
            if (isCritical)
            {
                Debug.Log("Dano crítico! " + damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
{
    if (atkPoint != null)
    {
        Gizmos.color = Color.red;  
        Gizmos.DrawWireSphere(atkPoint.position, atkRange);
    }
}
}
