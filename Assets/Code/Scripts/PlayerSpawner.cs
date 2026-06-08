using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [Header("Personagens (Prefabs)")]
    [Tooltip("O personagem que será o Host (quem cria a sala)")]
    [SerializeField] private NetworkPrefabRef playerPrefab;
    [Tooltip("O personagem que será o Cliente (quem entra na sala)")]
    [SerializeField] private NetworkPrefabRef player2Prefab;
    
    private NetworkRunner runner;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        runner = GetComponent<NetworkRunner>();

        if (runner != null)
        {
            runner.AddCallbacks(this);
        }
    }

    public void SpawnPlayer(PlayerRef player)
    {
        if (runner.GetPlayerObject(player) != null)
        {
            //Debug.Log("Jogador já possui um avatar na cena.");
            return;
        }

        //Debug.Log("Criando player: " + player);
        NetworkPrefabRef prefabToSpawn;
        if (runner.IsSharedModeMasterClient)
        {
            prefabToSpawn = playerPrefab;
            Debug.Log($"Criando Player 1 (Master): {player}");
        }
        else
        {
            prefabToSpawn = player2Prefab;
            Debug.Log($"Criando Player 2 (Cliente): {player}");
        }

        Vector3 spawnPos = new Vector3(
            UnityEngine.Random.Range(-3f, 3f),
            20f,
            UnityEngine.Random.Range(-3f, 3f)
        );

        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 50f))
        {
            spawnPos = new Vector3(0, 0.5f, 0);
            //Debug.Log("Raycast foi em:" + spawnPos);
        }
        else
        {
            spawnPos = new Vector3(0, 0.5f, 0);
            Debug.Log("Raycast fakhou");
        }

        NetworkObject playerObject = runner.Spawn(
            prefabToSpawn,
            spawnPos,
            Quaternion.identity,
            player
        );

        runner.SetPlayerObject(player, playerObject);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (player == runner.LocalPlayer)
        {
            SpawnPlayer(player);
        }
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        //Debug.Log("Cena carregada");

        if (runner.GetPlayerObject(runner.LocalPlayer) == null)
        {
            SpawnPlayer(runner.LocalPlayer);
        }
    }

    #region Unused Callbacks
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player){}
    public void OnInput(NetworkRunner runner, NetworkInput input){}
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input){}
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason){}
    public void OnConnectedToServer(NetworkRunner runner){}
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason disconnectReason){}
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token){}
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason){}
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message){}
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList){}
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data){}
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken){}
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data){}
    public void OnSceneLoadStart(NetworkRunner runner){}
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player){}
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress){}
    public void OnConnectedToServer(NetworkRunner runner, NetAddress remoteAddress){}
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessions, bool update){}
    #endregion
}