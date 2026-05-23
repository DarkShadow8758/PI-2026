using Fusion;
using UnityEngine;

public abstract class DefaultCharacter : NetworkBehaviour, IDamageCalculator
{
    [Header("Combat")]
    [SerializeField] protected float criticalChance = 0.2f;
    [SerializeField] protected float criticalMultiplier = 2f;

    [Header("Health")]
    [SerializeField] protected float maxHealth = 100f;
    private float previousHealth;

    [Networked, OnChangedRender(nameof(OnHealthChanged))]
    public float CurrentHealth { get; set; }

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            CurrentHealth = maxHealth;
        }
    }

    public virtual (float damage, bool isCritical) GetDamage(float baseDamage)
    {
        if (!HasStateAuthority) return (baseDamage, false);

        bool isCritical = Random.value < criticalChance;
        float finalDamage = isCritical ? baseDamage * criticalMultiplier : baseDamage;

        return (finalDamage, isCritical);
    }

    // RPC permite que qualquer jogador mande um aviso de dano para a Autoridade de Estado
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public virtual void Rpc_TakeDamage(float amount, bool isCritical = false)
    {
        CurrentHealth -= amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

        if (CurrentHealth <= 0)
        {
            Death();
        }
    }

    public virtual void GainLife(float amount)
    {
        if (!HasStateAuthority) return;

        CurrentHealth += amount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
    }

    

    // Chamado pelo Fusion em todos os clientes
    private void OnHealthChanged()
    {
        // Se a vida atual for menor que a antiga, tomou dano
        if (CurrentHealth < previousHealth)
        {
            float damageTaken = previousHealth - CurrentHealth;
            ShowDamageVisuals(damageTaken);
        }

        // Atualiza a memória
        previousHealth = CurrentHealth;
    }

    protected virtual void ShowDamageVisuals(float damageAmount)
    {
        // Deixe vazio aqui, o EnemyController vai sobrescrever isso
    }
    protected abstract void Death();
}