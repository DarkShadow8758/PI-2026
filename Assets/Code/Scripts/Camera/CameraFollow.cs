using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float rotationX = 0f; 
    [SerializeField] private float rotationY = 0f; 
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Fusion Settings")]
    [Tooltip("Coloque 0 para câmera travada no jogador, ou 10 para um leve atraso visual")]
    [SerializeField] private float cameraLag = 10f; 
    
    private Quaternion targetRotation;
    
    void Start()
    {
        targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
    }

    // REMOVEMOS O LateUpdate! 
    // O Fusion vai chamar essa função na hora perfeita.
    public void UpdateCameraNetwork()
    {
        if(target == null) return;

        Vector3 targetPos = target.position + offset;

        // Trocamos o SmoothDamp problemático por Lerp ou travamento direto
        if (cameraLag > 0)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * cameraLag);
        }
        else
        {
            transform.position = targetPos; // Câmera 100% travada e perfeitamente lisa
        }

        targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}