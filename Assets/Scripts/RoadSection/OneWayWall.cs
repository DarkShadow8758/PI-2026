using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayWall : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().isTrigger = false;
        }
    }
}
