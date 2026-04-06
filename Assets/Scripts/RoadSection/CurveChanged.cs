using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveChanged : MonoBehaviour
{
    public Material[] myMaterials;
    
    private float currentValue;
    private float targetValue;
    [SerializeField] private float maxValue = .005f, minValue = -.005f;
    public float lerpTime;
    private bool isComplete = true;

    private void Start()
    {
        foreach (Material material in myMaterials)
        {
            currentValue = material.GetFloat("_Sideways_Strenght");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Curve"))
        {
            if (isComplete)
            {
                StartCoroutine(ChangeCurveStrenght());
            }
        }
    }

    public IEnumerator ChangeCurveStrenght()
    {
        float elapsedTime = 0;
        targetValue = Random.Range(minValue, maxValue);
        Debug.Log(targetValue);
        while (elapsedTime < lerpTime)
        {
            isComplete = false;

            currentValue = Mathf.Lerp(currentValue, targetValue, elapsedTime / lerpTime);
            elapsedTime += Time.deltaTime;
 
            foreach (Material material in myMaterials)
            {
                material.SetFloat("_Sideways_Strenght", currentValue);
            }

            yield return null;
        }

        isComplete = true;
        currentValue = targetValue;
    }
}
