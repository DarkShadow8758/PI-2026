using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float spd;
    [Header("Attack")]
    [SerializeField] private Transform atkPoint;
    [SerializeField] private float atkRange;
    [SerializeField] private LayerMask enemyLayers;
    private Vector2 move;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    } 

    public void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log($"Botão pressionado! Fase: {context.phase}");
        if (context.performed)
        {
            Attack();
        }
    }
    
    void Update()
    {
        MovePlayer();
    }

    public void MovePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        transform.Translate(movement * spd * Time.deltaTime, Space.World);
    }

    public void Attack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(atkPoint.position, atkRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Acertou " + enemy.name);
        }
    }
}
