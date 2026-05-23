using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    [Networked]
    public int AlivePlayers { get; set; }

    private void Awake()
    {
        // Proteção padrão de Singleton para evitar duplicatas
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // O jogador (seja quem for) envia um RPC para a Autoridade de Estado do GameManager
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_RegisterPlayer()
    {
        AlivePlayers++;
        Debug.Log("Jogador registrado. Total vivos: " + AlivePlayers);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_PlayerDied()
    {
        AlivePlayers--;
        Debug.Log("Jogadores vivos: " + AlivePlayers);

        if (AlivePlayers <= 0)
        {
            AlivePlayers = 0;

            // Apenas a Autoridade de Estado pode pedir para o Runner 
            // recarregar a cena de forma sincronizada para todo mundo
            Runner.LoadScene(
                SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex)
            );
        }
    }
}