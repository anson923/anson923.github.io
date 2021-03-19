using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class ColorSystemOffline : MonoBehaviour
{
    private DragSystemOffline dragSystem;
    public GameObject oldSelect;
    public GameObject newSelect;
    public GameObject movingObject;
    public bool okToMove = false;
    public Text yourTurn;
    private TurnMenuOffline turnMenu;
    private SoccerSpeedControlOffline ballSpeed;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            turnMenu = FindObjectOfType<TurnMenuOffline>().GetComponent<TurnMenuOffline>();
            dragSystem = FindObjectOfType<DragSystemOffline>().GetComponent<DragSystemOffline>();
            ballSpeed = FindObjectOfType<SoccerSpeedControlOffline>().GetComponent<SoccerSpeedControlOffline>();
        }
        catch (Exception e)
        {
            //Avoid crash
        }


    }
    private void Update()
    {
        if(PlayerPrefs.GetInt("Mode",0) == 1)
        {
            if (dragSystem != null)
            {
                if (dragSystem.Shot == true)
                {
                    StartCoroutine(updates());
                }
            }
        }
        else if(PlayerPrefs.GetInt("Mode", 0) == 5)
        {
            if (Server.shot == true)
            {
                StartCoroutine(updates());
            }
        }

    }
    IEnumerator updates()
    {
        yield return new WaitForSeconds(0.2f);
        if (movingObject.GetComponent<Rigidbody>().velocity.magnitude == 0f && ballSpeed.GetComponent<Rigidbody>().velocity.magnitude <= 0f)
        {
            if(Server.isServer)
            {
                Server.showTurn = true;
            }
            else
            {
                Client.showTurnServer = true;
            }
            StartCoroutine(turnMenu.YourturnMenu());
            if (PlayerPrefs.GetInt("Mode", 0) == 1)
                dragSystem.Shot = false;
            else if (PlayerPrefs.GetInt("Mode", 0) == 5)
                Server.shot = false;
        }
        yield break;
    }

    //Change Selected player color
    public void ChangeColors()
    {
        try
        {
            if (PlayerPrefs.GetInt("Mode", 0) == 1 || PlayerPrefs.GetInt("Mode", 0) == 5)
            {
                int currentTurn = 0;
                if(PlayerPrefs.GetInt("Mode", 0) == 1)
                {
                    currentTurn = dragSystem.CurrentTurn;
                }
                else
                {
                    currentTurn = TurnMenuOffline.currentTurn;
                }

                if (newSelect.tag == "Player" && currentTurn == 1)
                //Change color to white
                newSelect.GetComponent<MeshRenderer>().material.color = Color.white;

                else if (newSelect.tag == "Enemy" && currentTurn == 0)
                    //Change color to pink
                    newSelect.GetComponent<MeshRenderer>().material.color = Color.yellow;

                //If there is old Object
                if (oldSelect == newSelect)
                {
                    ;
                }
                else if (oldSelect != null && oldSelect != newSelect)
                {
                    //Change the old Object to white
                    if (currentTurn == 1)
                        oldSelect.GetComponent<MeshRenderer>().material.color = Color.blue;
                    else if (currentTurn == 0)
                        oldSelect.GetComponent<MeshRenderer>().material.color = Color.red;
                }

                //Assign the new Selected Object TO old Selected Object
                oldSelect = newSelect;
            }
            else if ((PlayerPrefs.GetInt("Mode", 0) == 2 || PlayerPrefs.GetInt("Mode", 0) == 3) && okToMove)
            {
                if (newSelect.tag == "Player")
                    //Change color to white
                    newSelect.GetComponent<MeshRenderer>().material.color = Color.white;

                //If there is old Object
                if (oldSelect == newSelect)
                {
                    ;
                }
                else if (oldSelect != null && oldSelect != newSelect)
                {
                    //Change the old Object to white
                    oldSelect.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
            }


        }

        catch
        {
            Debug.Log("Select a player warning");
        }
    }



}

