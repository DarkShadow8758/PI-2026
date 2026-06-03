using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float destroyTime = 1.5f; // Recomendado menor no mobile para não poluir
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);
    [SerializeField] private Vector3 randomizeIntensity = new Vector3(0.5f, 0, 0);
    
    private Transform camTransform;

    void Start()
    {
        if (Camera.main != null) camTransform = Camera.main.transform;

        Destroy(gameObject, destroyTime);

        transform.position += offset;
        transform.position += new Vector3(
            Random.Range(-randomizeIntensity.x, randomizeIntensity.x),
            Random.Range(-randomizeIntensity.y, randomizeIntensity.y),
            Random.Range(-randomizeIntensity.z, randomizeIntensity.z)
        );
    }

    void LateUpdate()
    {
        if (camTransform != null)
        {
            Vector3 direction = camTransform.forward;
            direction.y = 0;
            transform.forward = direction;
        }
    }
}