using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotator : MonoBehaviour
{
    public Transform bicycle;
    public float spacing = 5.5f, verticalAdjust = 1.5f, horizontalAdjust = 0.8f;
    public float xRotationSpeed = 0.1f, yRotationSpeed = 0.1f;
    BicycleMan bman;

    Transform cam;
    float xAngle = 0, yAngle = 0;

    private void Start()
    {
        bman = FindObjectOfType<BicycleMan>();
        cam = transform.GetChild(0);
        
        transform.position = new Vector3(bicycle.position.x+horizontalAdjust, bicycle.position.y+verticalAdjust, bicycle.position.z);
        transform.eulerAngles = new Vector3(xAngle, yAngle, 0);

        cam.localPosition = new Vector3(0,0,-spacing);
    }
    // Update is called once per frame
    void Update()
    {
        if (!Level.paused)
        {
            if (!bman.dead)
                transform.position = new Vector3(bicycle.position.x + horizontalAdjust, bicycle.position.y + verticalAdjust, bicycle.position.z);

            // Vertical rotation
            if (Input.GetKey(KeyCode.W))
            {
                xAngle = Mathf.Min(90, xAngle + xRotationSpeed);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                xAngle = Mathf.Max(-10, xAngle - xRotationSpeed);
            }

            // Horizontal rotation
            if (Input.GetKey(KeyCode.D))
            {
                yAngle -= yRotationSpeed;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                yAngle += yRotationSpeed;
            }

            transform.eulerAngles = new Vector3(xAngle, yAngle, 0);
        }
    }

}
