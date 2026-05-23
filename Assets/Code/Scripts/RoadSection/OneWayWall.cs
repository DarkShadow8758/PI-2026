using Fusion;
using UnityEngine;

public class OneWayWall : NetworkBehaviour
{
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private Collider backWall;
    void OnTriggerExit(Collider other)
    {
        if (!HasStateAuthority)
            return;
        if (other.CompareTag("Player"))
        {
            if (backWall.isTrigger == false)
            {
                return;
            }

            backWall.isTrigger = false;
            spawner.waveStart = true;
        }
    }
}
