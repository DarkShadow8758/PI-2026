using Fusion;
using UnityEngine;

public class SectionTrigger : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Garante que só o seu personagem local avise o LevelManager (evita envios duplos)
        if (!HasStateAuthority) return;

        if (other.CompareTag("Trigger"))
        {
            // Pega a posição exata (Z) deste trigger
            float triggerPositionZ = other.transform.position.z;

            // Desliga o trigger apenas na sua tela para não bater nele duas vezes no mesmo frame
            other.enabled = false;

            // Envia um pedido ao Gerenciador de Nível para spawnar a próxima pista
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.Rpc_RequestNextSection(triggerPositionZ);
            }
            else
            {
                Debug.LogWarning("LevelManager não encontrado na cena!");
            }
        }
    }
}