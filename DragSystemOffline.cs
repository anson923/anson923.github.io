using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSystemOffline : MonoBehaviour
{

    private Vector2 startPos, endPos, ForcePos;
    public float ReleasePower;
    private SoccerSpeedControlOffline speedControl;
    private LineDisplayOffline lineDisplay;
    private ColorSystemOffline colorSystem;
    private static bool shot = false;
    //Color Red Turn = 0 , Blue turn = 1;
    private static int playerTurn = 1;


    // Start is called before the first frame update
    void Start()
    {
        colorSystem = FindObjectOfType<ColorSystemOffline>().GetComponent<ColorSystemOffline>();
        lineDisplay = FindObjectOfType<LineDisplayOffline>().GetComponent<LineDisplayOffline>();
        colorSystem.movingObject = gameObject;
        if(speedControl != null)
            speedControl = FindObjectOfType<SoccerSpeedControlOffline>().GetComponent<SoccerSpeedControlOffline>();
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



    public bool Shot
    {
        get { return shot; }
        set { shot = value; }
    }

    public int CurrentTurn
    {
        get { return playerTurn; }
        set { playerTurn = value; }
    }




    // Update is called once per frame
    void Update()
    {
        if (speedControl == null)
        {
            try
            {
                speedControl = FindObjectOfType<SoccerSpeedControlOffline>().GetComponent<SoccerSpeedControlOffline>();
            }
            catch(Exception ex)
            {
                //Debug
            }
        }
            

        #region SpeedLimit Script
        //Speed Limit
        //If the speed is faster than 120, than limit the speed.
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > 110f && PlayerPrefs.GetInt("Mode",0) != 5)
        {
            //Limit speed to 120 maximum.
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity.normalized * 110f;
        }

        else if (gameObject.GetComponent<Rigidbody>().velocity.magnitude > 150f && PlayerPrefs.GetInt("Mode", 0) == 5)
        {
            //Limit speed to 120 maximum.
            gameObject.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity.normalized * 150f;
        }

        //If the player is moving Very low then Stop
        if (gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 1)
        {
            //Debug.Log("Stopping Object");
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            //Debug.Log(this.gameObject.name + "Object is now stopped");
            if (colorSystem.movingObject.GetComponent<Rigidbody>().velocity.Equals(Vector3.zero))
            {
                Debug.Log("no object is moving!");
            }
        }
        //Only for Mode 5.
        //if(gameObject.GetComponent<Rigidbody>().velocity.magnitude >= 1 && PlayerPrefs.GetInt("Mode",0) == 5)
        //{
        //    colorSystem.movingObject = gameObject;
        //}
        #endregion


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
                //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //if (change.newSelect != null)
                //{
                //    startPos = Input.mousePosition;
                //    Debug.LogWarning("Start Mouse Position = " + startPos);
                //}
                //else
                //{
                //    return;
                //}

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

                int currentTurn = 0;
                if (PlayerPrefs.GetInt("Mode", 0) == 5)
                {
                    currentTurn = TurnMenuOffline.currentTurn;
                }
                else
                {
                    currentTurn = CurrentTurn;
                }

                if ((currentTurn == 1  && gameObject.GetComponent<MeshRenderer>().material.color != Color.white || currentTurn == 0 && gameObject.GetComponent<MeshRenderer>().material.color != Color.yellow) || ForcePos == Vector2.zero)
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

                    if (newSelectedSpeed.velocity == Vector3.zero && movingObjectSpeed.velocity == Vector3.zero )
                    {
                        
                        if(PlayerPrefs.GetInt("Mode", 0) != 5 )
                            colorSystem.newSelect.GetComponent<Rigidbody>().AddForce(new Vector3(ForcePos.y * ReleasePower * Time.deltaTime, 0, ForcePos.x * -1 * ReleasePower * Time.deltaTime), ForceMode.Force);
                        else
                        {
                            if(Server.isServer)
                            {
                                colorSystem.newSelect.GetComponent<Rigidbody>().AddForce(new Vector3(ForcePos.y * ReleasePower * Time.deltaTime, 0, ForcePos.x * -1 * ReleasePower * Time.deltaTime), ForceMode.Force);
                            }
                            else
                            {
                                Client.tempForcePlayer = colorSystem.newSelect;
                                Client.tempForce = new Vector3(ForcePos.y * ReleasePower * Time.deltaTime, 0, ForcePos.x * -1 * ReleasePower * Time.deltaTime);
                            }
                        }

                        if (PlayerPrefs.GetInt("Mode", 0) == 1)
                        {
                            Shot = true;
                            if (CurrentTurn == 1)
                            {
                                colorSystem.newSelect.GetComponent<MeshRenderer>().material.color = Color.blue;      
                                 //For Next team round
                                 CurrentTurn = 0;
                            }
                            else if (CurrentTurn == 0)
                            {
                                colorSystem.newSelect.GetComponent<MeshRenderer>().material.color = Color.red;
                                //For Next team round
                                CurrentTurn = 1;
                            }  
                        }
                        else if (PlayerPrefs.GetInt("Mode", 0) == 2 || PlayerPrefs.GetInt("Mode", 0) == 3)
                        {
                            colorSystem.newSelect.GetComponent<MeshRenderer>().material.color = Color.blue;
                        }
                        //TODO: Only for TCP network.
                        else if (PlayerPrefs.GetInt("Mode", 0) == 5)
                        {
                            if(Server.isServer)
                            {
                                Server.shot = true;
                                Server.UpdateShot();
                            }
                            else
                            {
                                Server.shot = true;
                                Client.UpdateShot();
                            }
                                
                                
                            if (TurnMenuOffline.currentTurn == 1)
                            {
                                colorSystem.newSelect.GetComponent<MeshRenderer>().material.color = Color.blue;
                                //For Next team round
                                TurnMenuOffline.currentTurn = 0;
                            }
                            else if (TurnMenuOffline.currentTurn == 0)
                            {
                                colorSystem.newSelect.GetComponent<MeshRenderer>().material.color = Color.red;
                                //For Next team round
                                TurnMenuOffline.currentTurn = 1;
                            }
                            if (Server.isServer)
                            {
                                Server.UpdateCurrentTurn();
                            }
                                
                            else
                            {
                                Client.UpdateCurrentTurn();
                            }
                                
                            //if (Server.serverIsRunning)
                            //{
                            //    Server.force = new Vector3(ForcePos.y * ReleasePower * Time.deltaTime, 0, ForcePos.x * -1 * ReleasePower * Time.deltaTime);
                            //    Server.player = colorSystem.newSelect;
                            //}
                            
                        }


                        colorSystem.newSelect = null;
                        colorSystem.oldSelect = null;
                        lineDisplay.line.gameObject.SetActive(false);
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
            Debug.LogWarning("Waiting for newSelected Obecj assign! But it si fine if none selected");
        }
        #endregion


    }
}
