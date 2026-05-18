using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionTrigger : MonoBehaviour
{
    public float moveStep;
    public float stepCount = 0;
    public GameObject roadSection;
    private List<GameObject> sections = new();

    private bool spawned = false;

   

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Trigger"))// || spawned)
            return;

        //spawned = true;

        stepCount++;

        Vector3 spawnPos = new Vector3(
            0,
            0,
            moveStep * stepCount
        );

        GameObject newSection = Instantiate(
            roadSection,
            spawnPos,
            Quaternion.identity
        );

        sections.Add(newSection);

        if (sections.Count > 4)
        {
            GameObject oldest = sections[0];

            sections.RemoveAt(0);

            Destroy(oldest);
        }
        other.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Trigger"))
        {
            //spawned = false;
        }
    }
}