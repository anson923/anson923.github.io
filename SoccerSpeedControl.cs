using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class SoccerSpeedControl : NetworkBehaviour

{
    public Rigidbody rb;
    [SyncVar]
    public GameObject lastTouch;
    [SyncVar]
    public Vector3 originPos;
    void Awake()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        originPos = gameObject.GetComponent<Rigidbody>().position;
    }
    private void Update()
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
    private void OnCollisionEnter(Collision collision)
    {
        LastTouch(collision.gameObject);
    }


    //Set Last touch to server
    #region LastTouch
    public void LastTouch(GameObject gameObject)
    {
        CmdLastTouch(gameObject);
    }

    [Command(ignoreAuthority = true)]
    void CmdLastTouch(GameObject gameObject)
    {
        RpcLastTouch(gameObject);
    }

    [ClientRpc]
    public void RpcLastTouch(GameObject gameObject)
    {
        lastTouch = gameObject;
    }
    #endregion
}