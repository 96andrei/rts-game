using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    [SerializeField]
    float speed = 5;

    [SerializeField]
    float minY;
    [SerializeField]
    float maxY;

    [SerializeField]
    float zoomSpeed = 5;
    float zoom = 0;

    Vector3 movement;

    void LateUpdate () {

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        movement.x = h * speed;
        movement.z = v * speed;
        transform.position = transform.position + movement * Time.deltaTime;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if(scroll != 0)
        {
            zoom += zoomSpeed * Time.unscaledDeltaTime;
            transform.Translate(transform.forward * scroll * zoomSpeed);
            var pos = transform.position;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            transform.position = pos;
        }

	}
}
