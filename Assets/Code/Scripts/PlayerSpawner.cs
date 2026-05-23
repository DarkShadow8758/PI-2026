using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private NetworkPrefabRef playerPrefab;
    
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
        // Trava de segurança: evita spawn duplo caso a cena carregue duas vezes
        if (runner.GetPlayerObject(player) != null)
        {
            Debug.Log("Jogador já possui um avatar na cena.");
            return;
        }

        Debug.Log("Criando player: " + player);

        // Sorteia posição inicial para o Raycast
        Vector3 spawnPos = new Vector3(
            UnityEngine.Random.Range(-3f, 3f),
            20f,
            UnityEngine.Random.Range(-3f, 3f)
        );

        // Raycast para achar o chão (se houver variação no terreno)
        if (Physics.Raycast(spawnPos, Vector3.down, out RaycastHit hit, 50f))
        {
            spawnPos = new Vector3(0, 0.5f, 0);
            //spawnPos = hit.point + new Vector3(0, -1f, 0);
            Debug.Log("Raycast foi em:" + spawnPos);
        }
        else
        {
            spawnPos = new Vector3(0, 0.5f, 0);
            Debug.Log("Raycast fakhou");
        }

        // O jogador que chama runner.Spawn ganha a Autoridade de Estado
        NetworkObject playerObject = runner.Spawn(
            playerPrefab,
            spawnPos,
            Quaternion.identity,
            player
        );

        // Registra qual objeto pertence a qual jogador
        runner.SetPlayerObject(player, playerObject);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // NO SHARED/SINGLE MODE: Cada jogador é responsável por spawnar a si mesmo.
        if (player == runner.LocalPlayer)
        {
            SpawnPlayer(player);
        }
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        Debug.Log("Cena carregada");

        // TODO RESOLVIDO: Após a troca de cena, o próprio jogador verifica 
        // se ele está sem avatar e recria o seu. Sem depender do MasterClient.
        if (runner.GetPlayerObject(runner.LocalPlayer) == null)
        {
            SpawnPlayer(runner.LocalPlayer);
        }
    }

    // ==========================================
    // CALLBACKS NÃO UTILIZADOS 
    // (Agrupados para manter o script limpo, mas cumprindo a Interface)
    // ==========================================
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