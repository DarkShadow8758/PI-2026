using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;

    private void LateUpdate()
    {
        Vector3 cameraPosition = _mainCamera .transform.position;

        //We only want to rotate on Yaxis: 
        cameraPosition.y = transform.position.y;
        //Make the sprite face the camera
        transform.LookAt(cameraPosition);
        //Rorate 180 on Y because of Sprite Renderer works
        transform.Rotate(0f, 180f, 0f);
    }
}
