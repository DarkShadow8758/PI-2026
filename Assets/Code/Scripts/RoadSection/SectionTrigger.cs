using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class SectionTrigger : NetworkBehaviour
{
    public float moveStep;
    public float stepCount = 0;
    public GameObject roadSection;
    private List<NetworkObject> sections = new();

    private void OnTriggerEnter(Collider other)
    {
        if (!HasStateAuthority)
            return;

        if (!other.CompareTag("Trigger"))
            return;

        stepCount++;

        Vector3 spawnPos = new Vector3(0, 0, moveStep * stepCount);

        NetworkObject newSection = Runner.Spawn(roadSection, spawnPos, Quaternion.identity);

        sections.Add(newSection);

        if (sections.Count > 4)
        {
            NetworkObject oldest = sections[0];

            sections.RemoveAt(0);

            Runner.Despawn(oldest);
        }
        other.enabled = false;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Trigger"))
        {
            
        }
    }
}