using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayWall : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().isTrigger = false;
            spawner.waveStart = true;
            Debug.Log("player passou");
        }
    }
}
