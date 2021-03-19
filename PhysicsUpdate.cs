using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PhysicsUpdate : NetworkBehaviour
{
    public Rigidbody rb;
    //public Transform rbt;

    [SyncVar]//all the essental varibles of a rigidbody
    public Vector3 Velocity;
    [SyncVar]
    public Quaternion Rotation;
    [SyncVar]
    public Vector3 Position;
    [SyncVar]
    public Vector3 AngularVelocity;

    public uint targetID;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        //rbt = gameObject.GetComponent<Transform>();
        Position = rb.position;
        Rotation = rb.rotation;
        Velocity = rb.velocity;
        AngularVelocity = rb.angularVelocity;
        
    }
    void Update()
    {
        try
        {

            if (GetComponent<NetworkIdentity>().isServer)//if we are the server update the varibles with our cubes rigidbody info
            {
                UpdatePosition();
                rb.position = Position;
                rb.rotation = Rotation;
                rb.velocity = Velocity;
                rb.angularVelocity = AngularVelocity;
            }
            if (GetComponent<NetworkIdentity>().isClient)//if we are a client update our rigidbody with the servers rigidbody info
            {
                rb.position = Position + Velocity * (float)NetworkTime.rtt;//account for the lag and update our varibles
                rb.rotation = Rotation * Quaternion.Euler(AngularVelocity * (float)NetworkTime.rtt);
                rb.velocity = Velocity;
                rb.angularVelocity = AngularVelocity;
                Debug.Log("Position Update for client");
            }
            

        }
        catch (Exception ex)
        {
            //Debug.Log($"PhysicsUpdate Error - GameObject ({gameObject.name}) , Parent{gameObject.GetComponent<Transform>().parent.name} isServer: {gameObject.GetComponent<Transform>().parent.GetComponent<NetworkIdentity>().isServer} , isClient: {gameObject.GetComponent<Transform>().parent.GetComponent<NetworkIdentity>().isClient} \n With the Following Error {ex} !");
        }
        
    }

    //Force
    public void ApplyForce(Vector3 force, ForceMode FMode)//apply force on the client-side to reduce the appearance of lag and then apply it on the server-side
    {
        if (GetComponent<NetworkIdentity>().isServer)
            RpcApplyForce(force, FMode);
        else
            CmdApplyForce(force, FMode);
    }

    [Command]//function that runs on server when called by a client
    public void CmdApplyForce(Vector3 force, ForceMode FMode)
    {
        //rb.AddForce(force, FMode);//apply the force on the server side
        RpcApplyForce(force, FMode);
    }

    [ClientRpc]
    public void RpcApplyForce(Vector3 force, ForceMode FMode)
    {
        rb.AddForce(force, FMode);
        
    }


    //Position
    void UpdatePosition()
    {

        if (GetComponent<NetworkIdentity>().isServer)
        {
            RpcUpdatePosition();
            Debug.Log($"Updated Gameobject ({gameObject}) stats with RPC.");
        }
            
        else
        {
            CmdUpdatePosition();
            Debug.Log($"Updated Gameobject ({gameObject}) stats with Cmd.");
        }


    }

    [ClientRpc]
    void RpcUpdatePosition()
    {
        Position = rb.position;
        Rotation = rb.rotation;
        Velocity = rb.velocity;
        AngularVelocity = rb.angularVelocity;
    }

    [Command]
    void CmdUpdatePosition()
    {
        RpcUpdatePosition();
    }
}
