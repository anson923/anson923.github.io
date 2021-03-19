using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System;

public class DragSystem : NetworkBehaviour
{

    private Vector2 startPos, endPos, ForcePos;
    public float ReleasePower;
    private SoccerSpeedControl speedControl;
    private ControlPoint controllPoint;
    private ColorSystem colorSystem;
    public TurnCounter turnCounter;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        colorSystem = FindObjectOfType<ColorSystem>().GetComponent<ColorSystem>();
        speedControl = FindObjectOfType<SoccerSpeedControl>().GetComponent<SoccerSpeedControl>();
        controllPoint = FindObjectOfType<ControlPoint>().GetComponent<ControlPoint>();
        colorSystem.movingObject = gameObject;
        rb = gameObject.GetComponent<Rigidbody>();
    }

    //Access private member to other script.
    public Vector3 getStartPos
    {
        get { return startPos; }
    }

    public Vector3 getEndPos
    {
        get { return endPos; }
    }



    #region SpeedControl
    //[Command(ignoreAuthority = true)]
    ////Call from client, run it on server
    //private void CmdControlSpeed(float SpeedLimit)
    //{

    //    RpcControlSpeed(SpeedLimit);
    //}

    //[ClientRpc]
    //void RpcControlSpeed(float SpeedLimit)
    //{
    //    #region SpeedLimit Script
    //    //Speed Limit
    //    //If the speed is faster than 120, than limit the speed.
    //    try
    //    {
    //        if (SpeedLimit > 120f)
    //        {
    //            //Limit speed to 120 maximum.
    //            rb.velocity = rb.velocity.normalized * 120f;
    //        }

    //        //If the player is moving Very low then Stop
    //        if (SpeedLimit <= 1f)
    //        {
    //            //Debug.Log("Stopping Object");
    //            rb.velocity = Vector3.zero;
    //        }
    //    }
    //    catch(Exception ex)
    //    {
    //        Debug.LogWarning($"Control Speed Function error: {ex}");
    //    }

    //    #endregion
    //}

    //private void ControlSpeed()
    //{
    //    float Speed = rb.velocity.magnitude;
    //    CmdControlSpeed(Speed);
    //}
    #endregion

    // Update is called once per frame
    void Update()
    {
        //if (rb.gameObject.tag == "Player" || rb.gameObject.tag == "Enemy")
        //{
        //    if (GameObject.FindGameObjectsWithTag("Enemy").Length != 0 && GameObject.FindGameObjectsWithTag("Player").Length != 0)
        //        ControlSpeed();
        //}


        try
        {
            turnCounter = FindObjectOfType<TurnCounter>().GetComponent<TurnCounter>();
            speedControl = FindObjectOfType<SoccerSpeedControl>().GetComponent<SoccerSpeedControl>();
            
        }
        catch(Exception ex)
        {

        }
            
        if (((gameObject.tag == "Player" && isServer && isClient) || (gameObject.tag == "Enemy" && isClientOnly)) && speedControl != null)
        {
            DragAndShoot();
        }
        //ControlSpeed();
  
    }
    private void DragAndShoot()
    {
        //Do drag and release shoot
        #region drag and shoot script
        try
        {

            /*If released mouse button and startpos not equal 0,0 which is same as the reset values 
                 * ( Prevent Mouse hold while player is still moving , 
                 * player movement direction will affected by holding mouse button when no control allows and release when got control back)
                 * It will prevent player to shot to the unexpected direction.
                 */

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Left click down detected");
                //Set start position
                /*
                 * Check if it is click on an object.
                 * Require to click to an object to drag and release.
                 */
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "Player" || hit.transform.tag == "Enemy")
                    {
                        startPos = Input.mousePosition;
                        Debug.LogWarning("Start Mouse Position = " + startPos);
                    }
                    else
                    {
                        return;

                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (startPos == Vector2.zero)
                {
                    return;
                }
                else
                {
                    Debug.Log("Left clicked up , and startpos has got");
                    endPos = Input.mousePosition;
                    Debug.LogWarning("End Mouse Position = " + endPos);
                    ForcePos = startPos - endPos;

                }

                /*Add Force
                * It follow the world direction
                * This game only use X and Z direction
                * Y direction is jump in 3D , jump is not implement in this game.
                * -Camera Angle-
                * +X for upward , -X for downward , +Z for Left and -Z for Right Direction.
                */
                if ((turnCounter.currentTurn == 1 && gameObject.GetComponent<MeshRenderer>().material.color != Color.white || turnCounter.currentTurn == 0 && gameObject.GetComponent<MeshRenderer>().material.color != Color.yellow) || ForcePos == Vector2.zero)

                {
                    startPos = Vector2.zero;
                    endPos = Vector2.zero;
                    ForcePos = Vector2.zero;
                    return;
                }
                else
                {
                    Debug.Log("Trying to Add force");
                    Rigidbody newSelectedSpeed = colorSystem.newSelect.GetComponent<Rigidbody>();
                    Rigidbody movingObjectSpeed = colorSystem.movingObject.GetComponent<Rigidbody>();
                    Rigidbody ballSpeed = speedControl.rb;

                    if (newSelectedSpeed.velocity == Vector3.zero && movingObjectSpeed.velocity == Vector3.zero && ballSpeed.velocity == Vector3.zero)
                    {
                        //change.newSelect.GetComponent<PhysicsUpdate>().ApplyForce(new Vector3(ForcePos.y * ReleasePower * Time.deltaTime, 0, ForcePos.x * -1 * ReleasePower * Time.deltaTime), ForceMode.Force);
                        ApplyForce(colorSystem.newSelect,new Vector3(ForcePos.y * ReleasePower * Time.deltaTime, 0, ForcePos.x * -1 * ReleasePower * Time.deltaTime), ForceMode.Force);
                        turnCounter.DragShot(true);
                        if (turnCounter.currentTurn == 1)
                        {
                            colorSystem.newSelect.GetComponent<MeshRenderer>().material.color = Color.blue;
                            //For Next team round
                            SyncTurn(0);
                        }
                        else if (turnCounter.currentTurn == 0)
                        {
                            colorSystem.newSelect.GetComponent<MeshRenderer>().material.color = Color.red;
                            //For Next team round
                            SyncTurn(1);
                        }

                        colorSystem.newSelect = null;
                        colorSystem.oldSelect = null;
                        controllPoint.line.gameObject.SetActive(false);
                    }

                    //Reset Values
                    Debug.Log("Resetting values");
                    startPos = Vector2.zero;
                    endPos = Vector2.zero;
                    ForcePos = Vector2.zero;

                }
            }
        }
        catch (UnassignedReferenceException app)
        {
            Debug.LogWarning("Waiting for newSelected Obecj assign! But it is fine if none selected");
        }
        #endregion
    }
    public void SyncTurn(int ServerTurn)
    {
        if(GetComponent<NetworkIdentity>().isServer)
        {
            turnCounter.RpcSyncTurn(ServerTurn);
            Debug.Log($"Side - onServer turn is now{ServerTurn}");
        }
        else
        {
            turnCounter.CmdSyncTurn(ServerTurn);
            Debug.Log($"Side - onClient turn is now{ServerTurn}");
        }

    }

    void ApplyForce(GameObject go,Vector3 force, ForceMode forceMode)
    {
        if(GetComponent<NetworkIdentity>().isServer)
        {
            CmdApplyforce(go,force, forceMode);
        }
        else
        {
            colorSystem.newSelect.GetComponent<Rigidbody>().AddForce(new Vector3(ForcePos.y * ReleasePower * Time.deltaTime, 0, ForcePos.x * -1 * ReleasePower * Time.deltaTime), ForceMode.Force);
            CmdApplyforce(go,force, forceMode);

        }

    }



    [Command(ignoreAuthority = true)]
    void CmdApplyforce(GameObject gameObject, Vector3 force, ForceMode forceMode)
    {
        gameObject.GetComponent<Rigidbody>().AddForce(force, forceMode);
    }

}
