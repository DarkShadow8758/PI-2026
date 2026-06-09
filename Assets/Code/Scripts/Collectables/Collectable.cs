using Fusion;
using UnityEngine;

public class Collectable : NetworkBehaviour
{
    [Networked] private NetworkBool isCollected {get; set;}
    void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;
        if(other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                isCollected = true; 
                
                ApllyEffect(player);
                
                if (HasStateAuthority)
                {
                    Runner.Despawn(Object);
                }
                else
                {
                    Rpc_DestroyCollectable();
                }
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void Rpc_DestroyCollectable()
    {
        if (Object.IsValid)
        {
            Runner.Despawn(Object);
        }
    }
    
    public virtual void ApllyEffect(PlayerController target)
    {
        
    }
}
