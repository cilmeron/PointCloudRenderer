using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    public float translationSpeed = 60f;
    public float altitude = 20f;

    private Camera _camera;

    private Vector3 _forwardDir;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _forwardDir = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
    }

    void Update()
    {
        
        TranslateCamera();    
        if (Input.GetMouseButton(1))
        {
            float mousex = Input.GetAxis("Mouse X");
            float mousey = Input.GetAxis("Mouse Y");
            transform.Rotate(Vector3.up *(mousex) * 10f, Space.World);
            transform.Rotate(Vector3.right *(-mousey) * 10f);
        }
       
    }
    private void TranslateCamera()
    {
        float translationX = Input.GetAxis("Horizontal") * Time.deltaTime * translationSpeed;
        float translationZ = Input.GetAxis("Vertical") * Time.deltaTime * translationSpeed;
        Vector3 translation = new Vector3(translationX, 0f, translationZ);
        transform.Translate(translation, Space.Self);
    }
}