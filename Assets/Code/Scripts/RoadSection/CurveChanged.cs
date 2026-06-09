using Fusion;
using System.Collections;
using UnityEngine;

public class CurveChanged : NetworkBehaviour
{
    public Material[] myMaterials;
    
    private float currentValue;
    [Networked]
    private float targetValue {get; set;}
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
        if (!HasStateAuthority)
            return;

        if (other.gameObject.CompareTag("Curve"))
        {
            if (isComplete)
            {
                targetValue = Random.Range(minValue, maxValue);
                StartCoroutine(ChangeCurveStrenght());
            }
        }
    }

    public IEnumerator ChangeCurveStrenght()
    {
        isComplete = false;
        float elapsedTime = 0;
        //targetValue = Random.Range(minValue, maxValue);
        //Debug.Log("Curve: " + targetValue);
        while (elapsedTime < lerpTime)
        {
            //isComplete = false;

            currentValue = Mathf.Lerp(currentValue, targetValue, elapsedTime / lerpTime);
            elapsedTime += Runner.DeltaTime;
 
            foreach (Material material in myMaterials)
            {
                material.SetFloat("_Sideways_Strenght", currentValue);
            }

            yield return null;
        }
        
        currentValue = targetValue;
        isComplete = true;   
    }
}
