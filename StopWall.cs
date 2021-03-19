using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWall : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ball")
            collision.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(2f,0f,0f);
    }
}
