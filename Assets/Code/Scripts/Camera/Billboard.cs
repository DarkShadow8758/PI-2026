using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;

        if (_mainCamera == null)
        {
            Debug.LogError("Nenhuma câmera com tag MainCamera encontrada!");
        }
    }

    private void LateUpdate()
    {
        if (_mainCamera == null) return;
        Vector3 cameraPosition = _mainCamera .transform.position;

        //We only want to rotate on Yaxis: 
        cameraPosition.y = transform.position.y;
        //Make the sprite face the camera
        transform.LookAt(cameraPosition);
        //Rorate 180 on Y because of Sprite Renderer works
        transform.Rotate(0f, 180f, 0f);
    }
}
