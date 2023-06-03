using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speedZoom = 20f;
    private float originalValue, zoom;
    private Camera camera;

    void Awake()
    {
        camera = GetComponent<Camera>();
        originalValue = camera.fieldOfView;
    }

    void Update()
    {
        if(Input.GetButton("Fire2"))
            zoom -= speedZoom * Time.deltaTime;
        else zoom += speedZoom * Time.deltaTime;

        zoom = Mathf.Clamp(zoom, 30f, 60f);
        camera.fieldOfView = zoom;
    }
}
