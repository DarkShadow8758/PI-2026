using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Vector3 offset;
    
    // Parâmetros de Rotação
    [SerializeField] private float rotationX = 0f; // Ângulo vertical (pitch)
    [SerializeField] private float rotationY = 0f; // Ângulo horizontal (yaw)
    [SerializeField] private float rotationSpeed = 5f; // Velocidade da rotação suave
    
    private Vector3 velocity = Vector3.zero;
    private Quaternion targetRotation;
    
    // Start is called before the first frame update
    void Start()
    {
        targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position + offset;

            // Posição suave
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

            // Rotação suave baseada nos parâmetros
            targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
