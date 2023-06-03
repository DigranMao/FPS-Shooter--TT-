using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraMenu : MonoBehaviour
{
    [SerializeField] private float speedRotate = 10;

    void Update()
    {
        transform.Rotate(Vector3.up, -speedRotate * Time.deltaTime);
    }
}
