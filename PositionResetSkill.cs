using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionResetSkill : MonoBehaviour
{
    List<GameObject> players = new List<GameObject>();
    List<GameObject> enemies = new List<GameObject>();
    public Button blueTeamBtn;
    public Button redTeamBtn;
    public GameObject redCDPanel;
    public GameObject blueCDPanel;
    public Text redCDText;
    public Text blueCDText;
    private TurnMenuOffline turnMenu;
    private DragSystemOffline dragSystem;
    private int blueCD;
    private int redCD;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(obj.gameObject);
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(obj.gameObject);
        }

        blueTeamBtn.onClick.AddListener(BlueTeamReset);
        redTeamBtn.onClick.AddListener(RedTeamReset);
        //Always start from Blue Team
        redTeamBtn.gameObject.SetActive(false);
        redCDPanel.gameObject.SetActive(false);
        blueCDPanel.gameObject.SetActive(false);
        
        
        if(PlayerPrefs.GetInt("Mode",0) != 4)
        {
            dragSystem = FindObjectOfType<DragSystemOffline>().GetComponent<DragSystemOffline>();
            turnMenu = FindObjectOfType<TurnMenuOffline>().GetComponent<TurnMenuOffline>();
        }
            

        
    }
    // Update is called once per frame
    //TODO: Update send a message to the server
    void Update()
    {

        if (PlayerPrefs.GetInt("Mode", 0) != 4)
        {
            //Set Mode turn.
            int CurrentTurn = 0;
            if(PlayerPrefs.GetInt("Mode", 0) != 5)
            {
                CurrentTurn = dragSystem.CurrentTurn;
                //Set visability.
                if (CurrentTurn == 0)
                {
                    redTeamBtn.gameObject.SetActive(true);
                    blueTeamBtn.gameObject.SetActive(false);
                }
                else
                {
                    blueTeamBtn.gameObject.SetActive(true);
                    redTeamBtn.gameObject.SetActive(false);
                }
            }
            else
            {
                CurrentTurn = TurnMenuOffline.currentTurn;
            }

            //Set visability.
            if(PlayerPrefs.GetInt("Mode", 0) == 5)
            {
                if(Server.isServer)
                {
                    blueTeamBtn.gameObject.SetActive(true);
                    redTeamBtn.gameObject.SetActive(false);
                }
                else
                {
                    redTeamBtn.gameObject.SetActive(true);
                    blueTeamBtn.gameObject.SetActive(false);
                }
            }
            


            //CD conditions
            if (CurrentTurn == 0)
            {
                redCD = PlayerPrefs.GetInt("RedCD", 0);

                if (redCD > 0)
                {
                    redCDPanel.gameObject.SetActive(true);
                    redTeamBtn.interactable = false;
                    redCDText.text = redCD.ToString();
                }
                else if (redCD == 0)
                {
                    redCDPanel.gameObject.SetActive(false);
                    redTeamBtn.interactable = true;
                }
            }
            else
            {
                blueCD = PlayerPrefs.GetInt("BlueCD", 0);
                if (blueCD > 0)
                {
                    blueCDPanel.gameObject.SetActive(true);
                    blueTeamBtn.interactable = false;
                    blueCDText.text = blueCD.ToString();
                }
                else if (blueCD == 0)
                {
                    blueCDPanel.gameObject.SetActive(false);
                    blueTeamBtn.interactable = true;
                }
            }
        }
        
        

    }



    private void BlueTeamReset()
    {
        if(PlayerPrefs.GetInt("Mode", 0) != 4)
        {
            try
            {
                foreach (GameObject obj in players)
                {
                    obj.GetComponent<Transform>().localPosition = obj.GetComponent<SelectionOffline>().getOrigin;
                }
                
                if (PlayerPrefs.GetInt("Mode", 0) != 5)
                {
                    dragSystem.CurrentTurn = 0;
                }
                else
                {
                    TurnMenuOffline.currentTurn = 0;
                    if (Server.isServer)
                    {
                        Server.UpdateCurrentTurn();
                        Server.UpdateSkillPos();
                    }
                }
                
                StartCoroutine(turnMenu.YourturnMenu());
                if (blueCD == 0)
                {
                    PlayerPrefs.SetInt("BlueCD", 5);
                }

            }
            catch (Exception e)
            {
                //Bugs?
                //Debug.Log(e);
            }
        }
        
    }

    private void RedTeamReset()
    {
        if (PlayerPrefs.GetInt("Mode", 0) != 4)
        {
            try
            {
                foreach (GameObject obj in enemies)
                {
                    obj.GetComponent<Transform>().localPosition = obj.GetComponent<SelectionOffline>().getOrigin;

                }

                if (PlayerPrefs.GetInt("Mode", 0) != 5)
                {
                    dragSystem.CurrentTurn = 1;
              
                }
                else
                {
                    TurnMenuOffline.currentTurn = 1;
                    if (!Server.isServer)
                    {
                        Client.UpdateCurrentTurn();
                        Client.UpdateSkillPos();
                    }
                }
                StartCoroutine(turnMenu.YourturnMenu());
                if (redCD == 0)
                {
                    PlayerPrefs.SetInt("RedCD", 5);
                }
            }
            catch (Exception e)
            {
                //Bugs?
                //Debug.Log(e);
            }
        }
    }


}
