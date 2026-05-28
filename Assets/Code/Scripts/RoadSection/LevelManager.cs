using Fusion;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : NetworkBehaviour
{
    public static LevelManager Instance;

    [Header("Configurações da Pista")]
    public float moveStep = 100f; 
    
    // 🔴 MUDANÇA CRÍTICA: Usar NetworkPrefabRef em vez de GameObject
    // Isso obriga o Fusion a usar o sistema de rede para spawnar e replicar o objeto.
    public NetworkPrefabRef roadSectionPrefab; 

    [Networked] public int stepCount { get; set; }
    [Networked] public float furthestTriggerZ { get; set; } 

    // Em Shared Mode, essa lista fica apenas no Host. Se o Host sair, o novo Host não terá a lista.
    // Para resolver isso de forma robusta no futuro, faça as pistas se autodestruírem. 
    // Mas para o seu escopo atual, vamos blindar a lista contra erros.
    private List<NetworkObject> activeSections = new List<NetworkObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public override void FixedUpdateNetwork()
    {
        // Apenas quem tem autoridade sobre o LevelManager (O primeiro a entrar na sala) gerencia isso
        if (HasStateAuthority)
        {
            CleanupOldSections();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_RequestNextSection(float triggerZ)
    {
        // Impede duplicados para o mesmo gatilho
        if (stepCount > 0 && triggerZ <= furthestTriggerZ)
        {
            return; 
        }

        furthestTriggerZ = triggerZ;
        stepCount++;

        Vector3 spawnPos = new Vector3(0, 0, moveStep * stepCount);

        // O Spawn agora usa o NetworkPrefabRef, garantindo que o Jogador 2 também veja a pista
        NetworkObject newSection = Runner.Spawn(roadSectionPrefab, spawnPos, Quaternion.identity);
        
        if (newSection != null)
        {
            activeSections.Add(newSection);
        }
    }

    private void CleanupOldSections()
    {
        // 💡 Dica de performance: Evite FindObjectsOfType no FixedUpdateNetwork se o jogo crescer.
        // O ideal seria manter uma lista de players conectados no seu GameManager.
        PlayerController[] players = FindObjectsOfType<PlayerController>();
        if (players.Length == 0) return;

        float lowestPlayerZ = float.MaxValue;
        foreach (PlayerController p in players)
        {
            if (p.transform.position.z < lowestPlayerZ)
            {
                lowestPlayerZ = p.transform.position.z;
            }
        }

        // Limpeza segura iterando de trás para frente
        for (int i = activeSections.Count - 1; i >= 0; i--)
        {
            NetworkObject section = activeSections[i];
            
            if (section != null && section.IsValid)
            {
                if (section.transform.position.z < (lowestPlayerZ - moveStep))
                {
                    activeSections.RemoveAt(i);
                    Runner.Despawn(section);
                }
            }
            else
            {
                // Remove referências nulas da lista caso a pista tenha sido destruída de outra forma
                activeSections.RemoveAt(i);
            }
        }
    }
}