using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private string name;
    private string tag;
    private Vector3 force;
    private Vector3 position;
    private Vector3 originalPosition;

    public Vector3 Force
    {
        get { return force; }
        set { force = value; }
    }

    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }


    public Vector3 OriginalPosition
    {
        get { return originalPosition; }
        set { originalPosition = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        name = gameObject.name;
        tag = gameObject.tag;
        force = Vector3.zero;
        Position = gameObject.GetComponent<Transform>().localPosition;
        OriginalPosition = Position;
    } 

    // Update is called once per frame
    void Update()
    {
        if (force != Vector3.zero)
        {
            Vector3 tempForce = force;
            gameObject.GetComponent<Rigidbody>().AddForce(tempForce, ForceMode.Force);
            Debug.Log($"GameClient - Added force!");
            force = Vector3.zero;
        }
    }

    public void AddPlayerForce(Vector3 pForce)
    {
        try
        {
            Force = pForce;
            Debug.Log($"GameClient - Player Class Added Force: {pForce}. It is now: {force}");
        }
        catch(Exception ex)
        {
            Debug.Log($"GameClient - Something wrong: {ex}");
        }
    }

    public void UpdatePosition(Vector3 pPos)
    {
        Position = pPos;
        gameObject.GetComponent<Transform>().localPosition = Position; 
    }
      
}
