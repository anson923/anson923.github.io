using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class TurnCounter : NetworkBehaviour
{
    public Image blueTurnHUD;
    public Image redTurnHUD;


    [SyncVar]
    public int currentTurn = 0;

    [SyncVar]
    public bool shot = false;

    [SyncVar]
    public bool turnMenu = true;

    [SyncVar]
    public int teamBlueScores = 0;

    [SyncVar]
    public int teamRedScores = 0;

    public bool Shot
    {
        get { return shot; }
        set { shot = value; }
    }

    // Start is called before the first frame update
    private void Start()
    {
        //Start From Team Blue
        //Color Red Turn = 0 , Blue turn = 1;
        currentTurn = 1;
    }
    private void Update()
    {
        Debug.Log($"Current Turn is : {currentTurn}");
        if(blueTurnHUD && redTurnHUD != null)
        {
            CmdSetHUD();
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdSetHUD()
    {
        RpcSetHUD();
    }

    [ClientRpc]
    public void RpcSetHUD()
    {
        //current 1 is Blue team. current 0 is Red team.
        if (currentTurn == 1)
        {
            blueTurnHUD.gameObject.SetActive(true);
            redTurnHUD.gameObject.SetActive(false);
        }
        else
        {
            blueTurnHUD.gameObject.SetActive(false);
            redTurnHUD.gameObject.SetActive(true);
        }

    }




    //TurnSync
    #region TurnSync
    [Command(ignoreAuthority = true)]
    public void CmdSyncTurn(int newTurn)
    {
        RpcSyncTurn(newTurn);
    }

    [ClientRpc]
    public void RpcSyncTurn(int newTurn)
    {
        currentTurn = newTurn;

    }
    #endregion

    //DragShot
    #region DragShot
    public void DragShot(bool shot)
    {
        CmdDragShot(shot);
    }

    [Command(ignoreAuthority = true)]
    public void CmdDragShot(bool shoot)
    {
        RpcDragShot(shoot);
    }

    [ClientRpc]
    public void RpcDragShot(bool shoot)
    {
        Shot = shoot;
    }
    #endregion

    //TurnDisplay
    #region TurnDisplay
    public void TurnDisplay(bool On)
    {
        CmdTurnDisplay(On);
    }

    [Command(ignoreAuthority = true)]
    void CmdTurnDisplay(bool On)
    {
        RpcTurnDisplay(On);
    }

    [ClientRpc]
    public void RpcTurnDisplay(bool On)
    {
        turnMenu = On;
    }
    #endregion


    //Team +1 score
    #region AddScore
    public void AddScore(bool team, int score)
    {
        CmdAddScore(team,score);
    }

    [Command(ignoreAuthority = true)]
    void CmdAddScore(bool team, int score)
    {
        RpcAddScore(team, score);
    }

    [ClientRpc]
    public void RpcAddScore(bool team, int score)
    {
        //True == blue team, False == red team
        if(team)
        {
            teamBlueScores += score;
        }
        else
        {
            teamRedScores += score;
        }
    }
    #endregion


}
