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
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_RegisterPlayer()
    {
        AlivePlayers++;
        //Debug.Log("Jogador registrado. Total vivos: " + AlivePlayers);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_PlayerDied()
    {
        AlivePlayers--;
        //Debug.Log("Jogadores vivos: " + AlivePlayers);

        if (AlivePlayers <= 0)
        {
            AlivePlayers = 0;

            Runner.LoadScene(
                SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex)
            );
        }
    }
}