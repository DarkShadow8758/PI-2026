using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : DefaultCharacter
{
    [Header("Targeting")]
    public string playerTag = "Player";
    public float chaseRange = 15f;

    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;

    private NavMeshAgent agent;
    private Transform target;

    [Header("Effetcs")]
    [SerializeField] private GameObject FloatingTextPrefab;
    [SerializeField] private Transform textPoint;

    [Header("Attack")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 1f;

    private bool canAttack = true;

    [Header("Drop")]
    [SerializeField] private DropManager dropManager;
    [SerializeField] private Transform dropPoint;

    EnemySpawner spawner;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("HostileAI precisa de um NavMeshAgent no mesmo GameObject.");
        }
        currentHealth = maxHealth;
    }

    private void Update()
    {
        UpdateTarget();
        if (target != null && Vector3.Distance(transform.position, target.position) <= chaseRange)
        {
            agent.SetDestination(target.position);
        }
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

    public void SetSpawner(EnemySpawner _spawner)
    {
       spawner = _spawner;
    }

    void ShowFloatingText(float value, Color textColor = default)
    {
        if (textColor == default) textColor = Color.white;
        var go = Instantiate(FloatingTextPrefab, textPoint.position, Quaternion.identity, transform);
        var tmp = go.GetComponent<TextMeshPro>();
        tmp.text = value.ToString();
        tmp.color = textColor;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && canAttack)
        { 
            PlayerController player =
                other.GetComponent<PlayerController>();

            if(player != null)
            {
                StartCoroutine(
                    DamageOverTime(player)
                );
            }
        }
    }
    private IEnumerator DamageOverTime(PlayerController player)
    {
        canAttack = false;

        player.TakeDamage(damage);

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;
    }

    public void TakeDamage(float amount, bool isCritical = false)
    {
        currentHealth -= amount;

        if (FloatingTextPrefab)
        {
            Color textColor = isCritical ? Color.red : Color.white;
            ShowFloatingText(amount, textColor);
        }
        

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        if(dropManager != null)
        {
            dropManager.TryDrop(dropPoint.position);
        }
        if (spawner != null) spawner.currentEnemy.Remove(this.gameObject);
        Destroy(gameObject);
    }

    
}

