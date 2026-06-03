using Fusion;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelManager : NetworkBehaviour
{
    public static LevelManager Instance;

    [Header("Configurações da Pista")]
    public float moveStep = 100f; 
    public NetworkPrefabRef roadSectionPrefab; 

    [Header("Configurações de Transição de Fase")]
    [Tooltip("Quantas pistas devem ser geradas antes de trocar de cena?")]
    public int maxSections = 10; 
    [Tooltip("O número (Build Index) da próxima cena no Build Settings.")]
    public int nextSceneIndex = 1;

    [Networked] public int stepCount { get; set; }
    [Networked] public float furthestTriggerZ { get; set; } 

    private List<NetworkObject> activeSections = new List<NetworkObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public override void FixedUpdateNetwork()
    {
        if (HasStateAuthority)
        {
            CleanupOldSections();
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_RequestNextSection(float triggerZ)
    {
        if (stepCount > 0 && triggerZ <= furthestTriggerZ)
        {
            return; 
        }
        
        if (stepCount >= maxSections)
        {
            Debug.Log($"Limite de {maxSections} seções atingido! Iniciando transição de cena...");
            
            Runner.LoadScene(SceneRef.FromIndex(nextSceneIndex));
            return; 
        }

        furthestTriggerZ = triggerZ;
        stepCount++;

        Vector3 spawnPos = new Vector3(0, 0, moveStep * stepCount);

        NetworkObject newSection = Runner.Spawn(roadSectionPrefab, spawnPos, Quaternion.identity);
        
        if (newSection != null)
        {
            activeSections.Add(newSection);
        }
    }

    private void CleanupOldSections()
    {
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
                activeSections.RemoveAt(i);
            }
        }
    }
}