using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            
            ApllyEffect(player);
            Destroy(gameObject);
        }
    }

    public virtual void ApllyEffect(PlayerController target)
    {
        
    }
}
