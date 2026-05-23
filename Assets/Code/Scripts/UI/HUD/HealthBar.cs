using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBarFillImage;
    [SerializeField] private Image healthBarTrailingFillImage;
    [SerializeField] private Image healthBarColor;
    [SerializeField] private float trailDelay = 0.5f;

    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color lowHealthColor = Color.red;

    private float maxHealth;
    private Sequence currentSequence;

    void Awake()
    {
        healthBarFillImage.fillAmount = 1f;
        healthBarTrailingFillImage.fillAmount = 1f;
    }

    public void SetMaxHealth(float value)
    {
        maxHealth = value;
        
        // Atualiza as cores e o preenchimento instantaneamente na largada,
        // evitando que a barra faça animação logo ao nascer.
        healthBarFillImage.fillAmount = 1f;
        healthBarTrailingFillImage.fillAmount = 1f;
        healthBarColor.color = fullHealthColor;
    }

    public void UpdateHealth(float currentHealth)
    {
        float ratio = currentHealth / maxHealth;
        Color currentColor = Color.Lerp(lowHealthColor, fullHealthColor, ratio);
        healthBarColor.color = currentColor;

        currentSequence?.Kill();
        currentSequence = DOTween.Sequence();

        currentSequence.Append(
            healthBarFillImage.DOFillAmount(ratio, 0.25f)
        );

        currentSequence.AppendInterval(trailDelay);

        currentSequence.Append(
            healthBarTrailingFillImage.DOFillAmount(ratio, 0.3f)
        );
    }

    // ESSENCIAL: Previne memory leak e erros de console (MissingReferenceException)
    // quando a cena é recarregada pelo GameManager e a HealthBar é destruída.
    private void OnDestroy()
    {
        currentSequence?.Kill();
    }
}