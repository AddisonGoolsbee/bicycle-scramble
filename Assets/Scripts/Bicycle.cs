using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bicycle : MonoBehaviour
{
    public Transform man;
    public float speed = 4f, pathSensitivity = 0.5f, turnAmount = 1f;
    public Vector3 bVelocity;

    BicycleMan bman;
    private Rigidbody rb, manrb;
    Vector3 prevPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        manrb = man.GetComponent<Rigidbody>();
        bman = FindObjectOfType<BicycleMan>();
        prevPosition = transform.position;
    }

    private void Update()
    {
        bVelocity = transform.position - prevPosition;
        prevPosition = bVelocity;
    }

    private void FixedUpdate()
    {
        // Normal gameplay
        if (!bman.dead)
        {
            Vector3 tempVect = transform.right * speed * Time.deltaTime;
            rb.MovePosition(transform.position + tempVect);

            // 'Pathfinding'
            // Move left
            if (transform.position.z < pathSensitivity) {
                if (transform.eulerAngles.y < 180)
                    transform.eulerAngles -= Vector3.up * turnAmount;
                else if (transform.eulerAngles.y < 350)
                    transform.eulerAngles += Vector3.up * turnAmount;
            } // Move Right
            else if (transform.position.z > pathSensitivity)
            {
                if (transform.eulerAngles.y > 180)
                    transform.eulerAngles += Vector3.up * turnAmount;
                else if (transform.eulerAngles.y > 10)
                    transform.eulerAngles -= Vector3.up * turnAmount;
            }


        }

        //Death
        else
            rb.constraints = RigidbodyConstraints.None;
    }
}
