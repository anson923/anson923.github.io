using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoalTextOnline : NetworkBehaviour
{
    public Text scoreText1;//TEAM NAME
    public Text scoreText2;//Scores
    public Text team1ScoreText;//0 blue
    public Text team2ScoreText;//0 red
    private TurnCounter counter;
    private NetworkManager networkManager;
    private SoccerSpeedControl soccer;
    private float delayTime = 1.5f;
    [SyncVar]
    public bool scoresPanel = false;
    [SyncVar]
    public string TeamName = "";
    [SyncVar]
    public string Message = "";


    // Start is called before the first frame update
    void Start()
    {
        scoreText1.gameObject.SetActive(false);
        scoreText2.gameObject.SetActive(false);
        counter = FindObjectOfType<TurnCounter>().GetComponent<TurnCounter>();
        networkManager = FindObjectOfType<NetworkManager>().GetComponent<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        team1ScoreText.text = counter.teamBlueScores.ToString();
        team2ScoreText.text = counter.teamRedScores.ToString();
        scoreText1.text = TeamName;
        scoreText2.text = Message;
        if(scoresPanel)
        {
            scoreText1.gameObject.SetActive(true);
            scoreText2.gameObject.SetActive(true);
        }
        else
        {
            scoreText1.gameObject.SetActive(false);
            scoreText2.gameObject.SetActive(false);
        }
        if(soccer == null)
            soccer = FindObjectOfType<SoccerSpeedControl>().GetComponent<SoccerSpeedControl>();
    }
    public IEnumerator CloseDisplayer()
    {
        yield return new WaitForSeconds(3f);
        ScorePanelDisplay(false);
        Debug.Log($"OnGoal text display disabled");
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            obj.GetComponent<Transform>().localPosition = obj.GetComponent<Selections>().getOrigin;
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            obj.GetComponent<Transform>().localPosition = obj.GetComponent<Selections>().getOrigin;
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("tempFail"))
        {

            obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
            obj.GetComponent<Rigidbody>().position = obj.GetComponent<SoccerSpeedControl>().originPos;
            CmdSoccerReset();
        }

        Debug.Log($"OnGoal - Server reset all player positions.");

        yield break;
    }

    public void OnGoal(string team, string message)
    {
        CmdOnGoal(team, message);
    }

    [Command(ignoreAuthority = true)]
    void CmdOnGoal(string team, string message)
    {
        RpcOnGoal(team, message);
    }

    [ClientRpc]
    void RpcOnGoal(string team, string message)
    {
        TeamName = team;
        Message = message;
        scoresPanel = true;
        StartCoroutine(CloseDisplayer());
        Debug.Log($"OnGoal - Server ready to reset player position.");
        
        
    }

    //Set boolean to Server - Client
    #region ScorePanelDisplay
    public void ScorePanelDisplay(bool On)
    {
        CmdScorePanelDisplay(On);
    }

    [Command(ignoreAuthority = true)]
    void CmdScorePanelDisplay(bool On)
    {
        RpcScorePanelDisplay(On);
    }

    [ClientRpc]
    public void RpcScorePanelDisplay(bool On)
    {
        scoresPanel = On;
    }
    #endregion

    //Set tezt
    #region ScorePanelDisplay
    public void ScoreText(string team,string message)
    {
        CmdScoreText(team,message);
    }

    [Command(ignoreAuthority = true)]
    void CmdScoreText(string team, string message)
    {
        RpcScoreText(team, message);
    }

    [ClientRpc]
    public void RpcScoreText(string team, string message)
    {
        TeamName = team;
        Message = message;
    }
    #endregion


    //Set soccer pause
    [Command(ignoreAuthority = true)]
    public void CmdSoccerPause()
    {
        RpcSoccerPause();
    }

    [ClientRpc]
    public void RpcSoccerPause()
    {
        soccer.gameObject.tag = "tempFail";
    }

    //ReSet soccer 
    [Command(ignoreAuthority = true)]
    public void CmdSoccerReset()
    {
        RpcSoccerReset();
    }

    [ClientRpc]
    public void RpcSoccerReset()
    {
        soccer.gameObject.tag = "Ball";
    }
}
