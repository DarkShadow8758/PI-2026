using UnityEngine;

public interface IDamageCalculator
{
    (float damage, bool isCritical) GetDamage(float baseDamage);
}