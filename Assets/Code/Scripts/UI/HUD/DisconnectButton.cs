using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisconnectButton : MonoBehaviour
{
    public async void LeaveRoomAndReturnToMenu()
    {
        NetworkRunner runner = FindObjectOfType<NetworkRunner>();

        if (runner != null)
        {
            Debug.Log("Desconectando do servidor...");
            
            await runner.Shutdown();
        }

        PlayerSpawner spawner = FindObjectOfType<PlayerSpawner>();
        if (spawner != null)
        {
            Destroy(spawner.gameObject);
        }

        // 4. Retorna para o Menu Principal
        Debug.Log("Voltando para o Menu Principal!");
        SceneManager.LoadScene(0);
    }
}