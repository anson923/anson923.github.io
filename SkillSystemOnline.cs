using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class SkillSystemOnline : NetworkBehaviour
{
    List<GameObject> players = new List<GameObject>();
    List<GameObject> enemies = new List<GameObject>();
    public Button blueTeamBtn;
    public Button redTeamBtn;
    public GameObject redCDPanel;
    public GameObject blueCDPanel;
    public Text redCDText;
    public Text blueCDText;
    private TurnMenu turnMenu;
    private TurnCounter turnCounter;
    [SyncVar]
    public int blueCD;
    [SyncVar]
    public int redCD;
    [SyncVar]
    public int tempBlueCD;
    [SyncVar]
    public int tempRedCD;


    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(obj.gameObject);
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemies.Add(obj.gameObject);
        }

        
        
        //Always start from Blue Team
        if(isServer)
        {
            blueTeamBtn.gameObject.SetActive(true);
            blueCDPanel.gameObject.SetActive(false);
            blueTeamBtn.onClick.AddListener(BlueTeamReset);
        }
        else
        {
            redTeamBtn.gameObject.SetActive(true);
            redCDPanel.gameObject.SetActive(false);
            redTeamBtn.onClick.AddListener(RedTeamReset);
        }

        turnCounter = FindObjectOfType<TurnCounter>().GetComponent<TurnCounter>();
        turnMenu = FindObjectOfType<TurnMenu>().GetComponent<TurnMenu>();

    }
    // Update is called once per frame
    void Update()
    {
        if(players.Count == 0 || enemies.Count == 0)
        {
            if(players.Count == 0)
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
                {
                    players.Add(obj.gameObject);
                }
            }
            else
            {
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    enemies.Add(obj.gameObject);
                }
            }
        }
        else if(turnCounter == null)
        {
            turnCounter = FindObjectOfType<TurnCounter>().GetComponent<TurnCounter>();
        }
        else if(turnMenu == null)
        {
            turnMenu = FindObjectOfType<TurnMenu>().GetComponent<TurnMenu>();
        }

        //CD conditions
        //For Client-Side
        if (!isServer)
        {
            if (redCD > 0)
            {
                redCDPanel.gameObject.SetActive(true);
                redTeamBtn.interactable = false;
                redCDText.text = redCD.ToString();
                redCDText.gameObject.SetActive(true);
            }
            else if (redCD == 0)
            {
                redCDPanel.gameObject.SetActive(false);
                redTeamBtn.interactable = true;
            }
        }
        //For Server-Side
        else
        {
            if (blueCD > 0)
            {
                blueCDPanel.gameObject.SetActive(true);
                blueTeamBtn.interactable = false;
                blueCDText.text = blueCD.ToString();
                blueCDText.gameObject.SetActive(true);
            }
            else if (blueCD == 0)
            {
                blueCDPanel.gameObject.SetActive(false);
                blueTeamBtn.interactable = true;
            }
        }
    }

    private void BlueTeamReset()
    {
        try
        {
            if (blueCD == 0 && turnCounter.currentTurn == 1)
            {
                CmdSetCD(true, 5);
                //Turn 1 == blue, Set
                StartCoroutine(turnMenu.YourturnMenu(0));
                turnCounter.CmdSyncTurn(0);
                Teleport(true);
                
            }
        }
        catch (Exception e)
        {
            //Bugs?
            //Debug.Log(e);
        }
    }

    private void RedTeamReset()
    {
        try
        {
            if (redCD == 0 && turnCounter.currentTurn == 0)
            {
                CmdSetCD(false, 5);
                StartCoroutine(turnMenu.YourturnMenu(1));
                turnCounter.CmdSyncTurn(1);
                Teleport(false);
               
            }
        }
        catch (Exception e)
        {
            //Bugs?
            //Debug.Log(e);
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdSetCD(bool isPLayer,int countDown)
    {
        RpcSetCD(isPLayer, countDown);
    }

    [ClientRpc]
    void RpcSetCD(bool isPLayer, int countDown)
    {
        if(isPLayer)
        {
            blueCD = countDown;
        }
        else
        {
            redCD = countDown;
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdSetTempCD(bool isPLayer, int countDown)
    {
        RpcSetTempCD(isPLayer, countDown);
    }

    [ClientRpc]
    void RpcSetTempCD(bool isPLayer, int countDown)
    {
        if (isPLayer)
        {
            tempBlueCD = countDown;
        }
        else
        {
            tempRedCD = countDown;
        }
    }

    void Teleport(bool isPlayer)
    {
        CmdTeleport(isPlayer);
    }

    [Command(ignoreAuthority = true)]
    void CmdTeleport(bool isPlayer)
    {
        RpcTeleport(isPlayer);
    }

    [ClientRpc]
    void RpcTeleport(bool isPlayer)
    {
        if (isPlayer)
        {
            foreach (GameObject obj in players)
            {
                obj.GetComponent<Transform>().localPosition = obj.GetComponent<Selections>().getOrigin;
            }
            Debug.Log($"Updated PositionXYZ - Server:Player Updated position.");
        }
        else
        {
            foreach (GameObject obj in enemies)
            {
                obj.GetComponent<Transform>().localPosition = obj.GetComponent<Selections>().getOrigin;
            }
            Debug.Log($"Updated PositionXYZ - Client:Player Updated position.");
        }
    }
}
