using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camTransform;

    private void LateUpdate()
    {
        // Cacheia a câmera para economizar performance (FindObject é pesado)
        if (camTransform == null)
        {
            if (Camera.main != null) camTransform = Camera.main.transform;
            return; 
        }

        // Pega a direção exata para onde a câmera está olhando
        Vector3 direction = camTransform.forward;
        
        // Zera o eixo Y para a barra não inclinar para trás/frente
        direction.y = 0; 

        // Força a barra a ficar perfeitamente paralela à tela. Zero tremedeira.
        transform.forward = direction;
    }
}