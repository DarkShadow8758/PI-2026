using Fusion;
using UnityEngine;

public class OneWayWall : NetworkBehaviour
{
    [SerializeField] private EnemySpawner spawner;
    void OnTriggerExit(Collider other)
    {
        if (!HasStateAuthority)
            return;
        if (other.CompareTag("Player"))
        {
            spawner.waveStart = true;
        }
    }
}
