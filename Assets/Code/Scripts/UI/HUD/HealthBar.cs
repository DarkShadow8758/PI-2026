
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
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            DrainHealthBar(10f);
            
        }
    }

    private void DrainHealthBar(float drainValue)
    {
        currentHealth -= drainValue;
        float ratio = currentHealth / maxHealth;

        Color currentColor = Color.Lerp(lowHealthColor, fullHealthColor, ratio);

        healthBarColor.color = currentColor;
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(healthBarFillImage.DOFillAmount(ratio, 0.25f)).SetEase(Ease.InOutSine);
        sequence.AppendInterval(trailDelay);
        sequence.Append(healthBarTrailingFillImage.DOFillAmount(ratio, 0.3f)).SetEase(Ease.InOutSine);

        sequence.Play();
    }
}
