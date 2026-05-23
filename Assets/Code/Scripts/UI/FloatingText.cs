using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float destroyTime = 1.5f; // Recomendado menor no mobile para não poluir
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);
    [SerializeField] private Vector3 randomizeIntensity = new Vector3(0.5f, 0, 0);
    
    private Transform camTransform;

    void Start()
    {
        // Pega a câmera local do jogador
        if (Camera.main != null) camTransform = Camera.main.transform;

        // Destrói localmente, não afeta a rede
        Destroy(gameObject, destroyTime);

        // Aplica o pulo e aleatoriedade
        transform.position += offset;
        transform.position += new Vector3(
            Random.Range(-randomizeIntensity.x, randomizeIntensity.x),
            Random.Range(-randomizeIntensity.y, randomizeIntensity.y),
            Random.Range(-randomizeIntensity.z, randomizeIntensity.z)
        );
    }

    void LateUpdate()
    {
        // Mantém o texto perfeitamente legível virado para a câmera do celular (Billboard embutido)
        if (camTransform != null)
        {
            Vector3 direction = camTransform.forward;
            direction.y = 0;
            transform.forward = direction;
        }
    }
}