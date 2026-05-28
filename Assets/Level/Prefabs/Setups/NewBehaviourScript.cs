using Fusion;
using UnityEngine;

public class RoadSectionNetwork : NetworkBehaviour
{
    public override void Spawned()
    {
        Debug.Log($"Nova pista spawnada e sincronizada na rede! Sou o Jogador com autoridade local? {HasStateAuthority}");
    }
}