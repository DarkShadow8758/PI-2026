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

    private void OnHealthChanged()
    {
        if (CurrentHealth < previousHealth)
        {
            float damageTaken = previousHealth - CurrentHealth;
            ShowDamageVisuals(damageTaken);
        }

        previousHealth = CurrentHealth;
    }

    protected virtual void ShowDamageVisuals(float damageAmount)
    {
    }
    protected abstract void Death();
}