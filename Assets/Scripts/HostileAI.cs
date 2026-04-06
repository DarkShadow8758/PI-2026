using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HostileAI : MonoBehaviour
{
    public enum AIState
    {
        Idle,
        Chase,
        Attack
    }

    [Header("Targeting")]
    public string playerTag = "Player";
    public float chaseRange = 15f;
    public float attackRange = 2f;

    [Header("Attack")]
    public float attackCooldown = 1f;
    public int damage = 10;

    private AIState currentState = AIState.Idle;
    private NavMeshAgent agent;
    private Transform target;
    private float attackTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("HostileAI precisa de um NavMeshAgent no mesmo GameObject.");
        }
        attackTimer = 0f;
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;

        UpdateTarget();
        UpdateStateMachine();
    }

    private void UpdateTarget()
    {
        //Find nearest player by Tag
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

    private void UpdateStateMachine()
    {
        if (target == null)
        {
            SetState(AIState.Idle);
            return;
        }

        float distance = Vector3.Distance(transform.position, target.position);

        switch (currentState)
        {
            case AIState.Idle:
                if (distance <= chaseRange)
                {
                    SetState(AIState.Chase);
                }
                break;

            case AIState.Chase:
                if (agent != null)
                {
                    agent.isStopped = false;
                    agent.SetDestination(target.position);
                }

                if (distance <= attackRange)
                {
                    SetState(AIState.Attack);
                }
                else if (distance > chaseRange)
                {
                    SetState(AIState.Idle);
                }
                break;

            case AIState.Attack:
                if (agent != null)
                {
                    agent.isStopped = true;
                }

                FaceTarget();

                if (distance > attackRange)
                {
                    SetState(AIState.Chase);
                }
                else
                {
                    TryAttack();
                }
                break;
        }
    }

    private void SetState(AIState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (currentState)
        {
            case AIState.Idle:
                if (agent != null) agent.isStopped = true;
                break;
            case AIState.Chase:
                if (agent != null) agent.isStopped = false;
                break;
            case AIState.Attack:
                if (agent != null) agent.isStopped = true;
                break;
        }
    }

    private void FaceTarget()
    {
        if (target == null) return;

        Vector3 dir = (target.position - transform.position).normalized;
        dir.y = 0f;
        if (dir.sqrMagnitude <= 0f) return;

        Quaternion lookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 10f);
    }

    private void TryAttack()
    {
        if (target == null) return;

        if (attackTimer >= attackCooldown)
        {
            attackTimer = 0f;
            Debug.Log($"{name} atacou {target.name} e causou {damage} de dano.");

            /*var health = target.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }*/
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

