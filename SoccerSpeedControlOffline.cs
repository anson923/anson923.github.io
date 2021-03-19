using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class SoccerSpeedControlOffline : MonoBehaviour

{
    public Rigidbody rb;
    public GameObject lastTouch;

    private Vector3 originalPosition;

    private Transform oringinalTransform;

    public Vector3 OriginalPosition
    {
        get { return originalPosition; }
        set { originalPosition = value; }
    }


    void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        OriginalPosition = gameObject.GetComponent<Transform>().localPosition;
    }
    private void Update()
    {
        if (PlayerPrefs.GetInt("Mode", 0) == 1)
        {
            if (rb.velocity.magnitude < 1)
            {
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
            }
            else if (rb.velocity.magnitude > 150f)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, 150f);
            }
        }
        else if (PlayerPrefs.GetInt("Mode", 0) == 2 || PlayerPrefs.GetInt("Mode", 0) == 3)
        {
            if (rb.velocity.magnitude < 1)
            {
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
            }
            else if (rb.velocity.magnitude > 55f)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, 55f);
            }
        }
        else if (PlayerPrefs.GetInt("Mode", 0) == 5)
        {
            if (rb.velocity.magnitude < 1)
            {
                rb.angularVelocity = Vector3.zero;
                rb.velocity = Vector3.zero;
            }
            else if (rb.velocity.magnitude > 200f)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, 200f);
            }
        }


    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            lastTouch = collision.gameObject;
            if (PlayerPrefs.GetInt("Mode", 0) == 5 && Server.isServer)
            {
                Server.UpdateLastTouch(lastTouch);
            }
            else if (PlayerPrefs.GetInt("Mode", 0) == 5 && !Server.isServer)
            {
                Client.UpdateLastTouch(lastTouch);
            }
        }

        
    }
}