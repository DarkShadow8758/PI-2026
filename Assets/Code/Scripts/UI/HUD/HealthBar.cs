
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image healthBarTrailingFillImage;
    [SerializeField] private Image healthBarColor;
    [SerializeField] private float trailDelay;

    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;

    private float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;

        healthBarFillImage.fillAmount = 1f;
        healthBarTrailingFillImage.fillAmount = 1f;
    } 

    public void GainLife(float gain)
    {
        currentHealth += gain;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        float ratio = currentHealth / maxHealth;

        Color currentColor = Color.Lerp(lowHealthColor, fullHealthColor, ratio);

        healthBarColor.color = currentColor;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(
            healthBarFillImage.DOFillAmount(ratio, 0.25f)
        );

        sequence.AppendInterval(trailDelay);

        sequence.Append(
            healthBarTrailingFillImage.DOFillAmount(ratio, 0.3f)
        );

        sequence.Play();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        float ratio = currentHealth / maxHealth;

        Color currentColor = Color.Lerp(lowHealthColor, fullHealthColor, ratio);

        healthBarColor.color = currentColor;

        Sequence sequence = DOTween.Sequence();

        sequence.Append(
            healthBarFillImage.DOFillAmount(ratio, 0.25f)
        );

        sequence.AppendInterval(trailDelay);

        sequence.Append(
            healthBarTrailingFillImage.DOFillAmount(ratio, 0.3f)
        );

        sequence.Play();

        if(currentHealth <= 0)
        {
            Debug.Log("Player morreu");
        }
    }
}
