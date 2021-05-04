using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleMan : MonoBehaviour
{
    Rigidbody rb;
    Transform body, head;

    public float uprightAdjustSpeed = 4f;
    public bool dead = false;
    public bool wonkyPhysics = false;
    public GameObject restartText;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        body = transform.GetChild(0);
        head = transform.GetChild(1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            wonkyPhysics = true;
    }

    private void FixedUpdate()
    {
        // Tries to stay upright
        if (!dead && Mathf.Abs(transform.rotation.x) > 1)
        {
            Vector3 newAngle = new Vector3(Mathf.LerpAngle(transform.rotation.x, 0, Time.deltaTime*uprightAdjustSpeed), 0, 0);
            transform.eulerAngles = newAngle;
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Ball"))
        //{
        //    var orthogonalVector = collision.contacts[0].point - transform.position;
        //    var collisionAngle = Vector3.Angle(orthogonalVector, rb.velocity);
        //}

        if (GetComponent<HingeJoint>() == null)
        {
            dead = true;
            if(!wonkyPhysics)
                rb.useGravity = true;
            Invoke("showRestartText", 3f);
        }
        
    }

    private void showRestartText()
    {
        restartText.SetActive(true);
    }
}
