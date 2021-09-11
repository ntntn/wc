using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    Camera cam;

    private void Awake() {
        cam = Camera.main;
    }
    void Update()
    {
        Vector3 pos = cam.transform.position;
        if (Input.mousePosition.y >= Screen.height - panBorderThickness) {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness) {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness) {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness) {
            pos.x -= panSpeed * Time.deltaTime;
        }

        cam.transform.position = pos;
    }
}
