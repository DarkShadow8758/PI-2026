using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float rotationX = 0f; 
    [SerializeField] private float rotationY = 0f; 
    [SerializeField] private float rotationSpeed = 5f;
    
    private Vector3 velocity = Vector3.zero;
    private Quaternion targetRotation;
    
    void Start()
    {
        targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 targetPos = target.position + offset;

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

            targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
