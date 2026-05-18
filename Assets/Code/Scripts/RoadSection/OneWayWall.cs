using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayWall : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private GameObject backWall;
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (backWall.GetComponent<Collider>().isTrigger == false)
            {
                return;
            }

            backWall.GetComponent<Collider>().isTrigger = false;
            spawner.waveStart = true;
        }
    }
}
