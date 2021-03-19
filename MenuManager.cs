using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MenuManager : MonoBehaviour
{
    public Button StartNewGameBtn;
    public Button MainManuBtn;
    public Button howToPlayBtn;
    public Button creditPage;
    public Button tutorial;
    public Button singlePlayerBtn;
    public Button tutorial1;
    public Button tutorial2;
    public Button multiPlayerBtn;
    public Button TcpMultiPlayerBtn;
    public Button quitGameBtn;
    public Text TeamWinText;
    public GameObject panel;
    
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            if(StartNewGameBtn != null)
                StartNewGameBtn.onClick.AddListener(StartNewGame);
            if(howToPlayBtn != null)
                howToPlayBtn.onClick.AddListener(HowToPlay);
            if(MainManuBtn != null)
                MainManuBtn.onClick.AddListener(MainMenu);
            if (creditPage != null)
                creditPage.onClick.AddListener(CreditPage);
            if (tutorial != null)
                tutorial.onClick.AddListener(Tutorial);
            if (singlePlayerBtn != null)
                singlePlayerBtn.onClick.AddListener(SinglePlayerMode);
            if (tutorial1 != null)
                tutorial1.onClick.AddListener(BasicControl);
            if (tutorial2 != null)
                tutorial2.onClick.AddListener(SkillReset);
            if (multiPlayerBtn != null)
                multiPlayerBtn.onClick.AddListener(MultiPlayerMode);
            if (TcpMultiPlayerBtn != null)
                TcpMultiPlayerBtn.onClick.AddListener(TcpMultiPlayer);
            if (quitGameBtn != null)
                quitGameBtn.onClick.AddListener(QuitGame);

        }
        catch(Exception e)
        {
            //Avoid crash
        }
    }
    // Update is called once per frame
    void Update()
    {
        try
        {
            int TeamBlue = PlayerPrefs.GetInt("Team1", 0);
            int TeamRed = PlayerPrefs.GetInt("Team2", 0);
            if(PlayerPrefs.GetInt("Mode",0) == 1 || PlayerPrefs.GetInt("Mode", 0) == 4 || PlayerPrefs.GetInt("Mode", 0) == 5)
            {

                if (TeamBlue == 3)
                {
                    panel.GetComponent<Image>().color = Color.blue;
                    TeamWinText.text = "GameOver \n" + "Team Blue " + PlayerPrefs.GetInt("Team1", 0) + " : " + PlayerPrefs.GetInt("Team2", 0) + " Team Red \n" + "Team Blue Won!";
                }
                else if (TeamRed == 3)
                {
                    panel.GetComponent<Image>().color = Color.red;
                    TeamWinText.text = "GameOver \n" + "Team Blue " + PlayerPrefs.GetInt("Team1", 0) + " : " + PlayerPrefs.GetInt("Team2", 0) + " Team Red \n" + "Team Red Won!";
                }
                else
                {

                    if (TeamBlue > TeamRed)
                    {
                        panel.GetComponent<Image>().color = Color.blue;
                        TeamWinText.text = "Timeout \n" + "Team Blue " + PlayerPrefs.GetInt("Team1", 0) + " : " + PlayerPrefs.GetInt("Team2", 0) + " Team Red \n" + "Team Blue Won!";
                    }
                    else if (TeamRed > TeamBlue)
                    {
                        panel.GetComponent<Image>().color = Color.red;
                        TeamWinText.text = "Timeout \n" + "Team Blue " + PlayerPrefs.GetInt("Team1", 0) + " : " + PlayerPrefs.GetInt("Team2", 0) + " Team Red \n" + "Team Red Won!";
                    }
                    else
                    {
                        panel.GetComponent<Image>().color = Color.gray;
                        TeamWinText.text = "Timeout \n" + "Team Blue " + PlayerPrefs.GetInt("Team1", 0) + " : " + PlayerPrefs.GetInt("Team2", 0) + " Team Red \n" + "Draw!";
                        StartNewGameBtn.gameObject.SetActive(false);
                        multiPlayerBtn.gameObject.SetActive(true);
                    }



                }
                if(PlayerPrefs.GetInt("Mode", 0) == 5)
                {
                    multiPlayerBtn.gameObject.SetActive(false);
                    StartNewGameBtn.gameObject.SetActive(false);
                }
            }
            
        }
        catch (Exception e)
        {
            //Avoid crash
        }
    }

    private void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        //Set timer to 3mins
        PlayerPrefs.SetFloat("Timer", 180);
        PlayerPrefs.SetInt("Mode", 1);
        PlayerPrefs.SetInt("RedCD", 0);
        PlayerPrefs.SetInt("BlueCD", 0);
        PlayerPrefs.SetInt("Team1", 0);
        PlayerPrefs.SetInt("Team2", 0);
        SceneManager.LoadScene(1);
        
    }

    private void MainMenu()
    {
        PlayerPrefs.SetFloat("Timer", 0);
        PlayerPrefs.SetInt("Mode", 0);
        SceneManager.LoadScene(0);
        
    }

    private void HowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    private void CreditPage()
    {
        SceneManager.LoadScene("CreditMenu");
    }

    private void Tutorial()
    {
        SceneManager.LoadScene("TutorialStage");
    }

    private void BasicControl()
    {
        PlayerPrefs.SetInt("Mode", 2);
        SceneManager.LoadScene("Tutorial1");
    }

    private void SkillReset()
    {
        PlayerPrefs.SetInt("Mode", 3);
        SceneManager.LoadScene("Tutorial2");
    }

    private void SinglePlayerMode()
    {

    }

    private void MultiPlayerMode()
    {
        //Set timer to 3mins
        //TODO: 3mins
        PlayerPrefs.SetFloat("Timer", 180);
        PlayerPrefs.SetInt("Mode", 4);
        PlayerPrefs.SetInt("Team1", 0);
        PlayerPrefs.SetInt("Team2", 0);
        //TODO: skillmode
        PlayerPrefs.SetInt("RedCD", 0);
        PlayerPrefs.SetInt("BlueCD", 0);
        SceneManager.LoadScene("MultiPlayerGame");

    }

    private void TcpMultiPlayer()
    {
        //TODO: 3mins
        PlayerPrefs.SetFloat("Timer", 180);
        PlayerPrefs.SetInt("Mode", 5);
        PlayerPrefs.SetInt("Team1", 0);
        PlayerPrefs.SetInt("Team2", 0);
        PlayerPrefs.SetInt("Turn", 1);
        //TODO: skillmode
        PlayerPrefs.SetInt("RedCD", 0);
        PlayerPrefs.SetInt("BlueCD", 0);
        SceneManager.LoadScene("TcpGame");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
