using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using Mirror;

public class ColorSystem : NetworkBehaviour
{
    public GameObject oldSelect;
    public GameObject newSelect;
    public GameObject movingObject;
    public bool okToMove = false;
    public Text yourTurn;
    public Button okButton;
    public Image panel;
    public Canvas lobby;
    private TurnMenu turnMenu;
    private TurnCounter turnCounter;
    private SoccerSpeedControl ballSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            turnMenu = FindObjectOfType<TurnMenu>().GetComponent<TurnMenu>();
            turnCounter = FindObjectOfType<TurnCounter>().GetComponent<TurnCounter>();
            ballSpeed = FindObjectOfType<SoccerSpeedControl>().GetComponent<SoccerSpeedControl>();
        }
        catch(Exception e)
        {
            //Avoid crash
        }
        

    }
    private void Update()
    {
        if (turnCounter == null || turnMenu == null || ballSpeed == null)
        {
            try
            {
                turnCounter = FindObjectOfType<TurnCounter>().GetComponent<TurnCounter>();
                turnMenu = FindObjectOfType<TurnMenu>().GetComponent<TurnMenu>();
                ballSpeed = FindObjectOfType<SoccerSpeedControl>().GetComponent<SoccerSpeedControl>();
            }
            catch (Exception e)
            {
                //Debug.
            }
        }
        if (turnCounter != null)
        {
            if (turnCounter.Shot == true)
            {
                updates();
            }
        }
    }
    void updates()
    {
        //TODO: turnMenu currently not available for Mode 4 milti player mode.
        //if(PlayerPrefs.GetInt("Mode",0) == 4)
        if(!lobby.gameObject.active)
        {
            int turn = turnCounter.currentTurn;
            StartCoroutine(turnMenu.YourturnMenu(turn));
        }
    }



    //Change Selected player color
    public void ChangeColors(bool isLocalPlayer, int turn)
    {
        try
        {
            if(PlayerPrefs.GetInt("Mode", 0) == 4)
            {
                if(isLocalPlayer)
                {
                    //if (newSelect.tag == "Player" && shoot.currentTurn == 1)
                    if (newSelect.tag == "Player" && turn == 1)
                        //Change color to white
                        newSelect.GetComponent<MeshRenderer>().material.color = Color.white;

                    else if (newSelect.tag == "Enemy" && turn == 0)
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
                        if (turn == 1)
                            oldSelect.GetComponent<MeshRenderer>().material.color = Color.blue;
                        else if (turn == 0)
                            oldSelect.GetComponent<MeshRenderer>().material.color = Color.red;
                    }

                    //Assign the new Selected Object TO old Selected Object
                    oldSelect = newSelect;
                }
            }
            
        }
        
        catch
        {
            Debug.Log("Select a player warning");
        }
    }

}
