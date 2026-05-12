using UnityEngine;

public class DefaultCharacter : MonoBehaviour, IDamageCalculator
{
    [SerializeField] protected float criticalChance = 0.2f;
    [SerializeField] protected float criticalMultiplier = 2.0f;

    public (float damage, bool isCritical) GetDamage(float baseDamage)
    {
        bool isCritical = Random.value < criticalChance;
        float finalDamage = isCritical ? baseDamage * criticalMultiplier : baseDamage;
        return (finalDamage, isCritical);
    }
}