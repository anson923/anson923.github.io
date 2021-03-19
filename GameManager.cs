using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;

public class GameManager : MonoBehaviour
{
    public Text teamBlueScore;
    public Text teamRedScore;
    public Text TimeCount;
    public Text GoalText;
    public Text TurnText;
    public GameObject pauseMenu;
    public Button pauseBtn;
    public Button continueGameBtn;
    public Button mainMenuBtn;
    public Button disconnectBtn;

    float timeRemaining;
    bool timerIsRunning = false;
    // Start is called before the first frame update
    void Start()
    {
        // Starts the timer
        timerIsRunning = true;
        timeRemaining = PlayerPrefs.GetFloat("Timer", 0);
        if(pauseBtn != null)
            pauseBtn.onClick.AddListener(PauseMenu);
        if(continueGameBtn != null)
            continueGameBtn.onClick.AddListener(Continue);
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (disconnectBtn != null)
        {
            disconnectBtn.onClick.AddListener(DisconnectServer);
            disconnectBtn.gameObject.SetActive(true);
        }
            
    }



    // Update is called once per frame
    void Update()
    {
        int mode = PlayerPrefs.GetInt("Mode", 0);
        if (PlayerPrefs.GetInt("Mode",0) == 1 || PlayerPrefs.GetInt("Mode", 0) == 5)
        {
            int teamBlue = int.Parse(teamBlueScore.text.ToString());
            int temaRed = int.Parse(teamRedScore.text.ToString());
            //If any team reach 3 goals. They win!
            if (teamBlue == 3 || temaRed == 3 && (mode == 1 || mode == 5 ))
            {
                SceneManager.LoadScene(2);
            }
            Timer();
        }
        else if(mode == 4)
        {
            Timer();
        }
        
        
    }

    void Timer()
    {
        int mode = PlayerPrefs.GetInt("Mode", 0);
        if (timerIsRunning && (mode == 1 || mode == 4 || mode == 5))
        {
            if (timeRemaining > 0)
            {
                DisplayTime(timeRemaining);
                if (GoalText.IsActive() == false)
                {
                    if (TurnText.IsActive() == true)
                    {
                        return;
                    }
                    else
                    {
                        timeRemaining -= Time.deltaTime;
                        PlayerPrefs.SetFloat("Timer", timeRemaining);
                    }

                }
                else
                {
                    return;
                }

            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0f;
                PlayerPrefs.SetFloat("Timer", timeRemaining);
                timerIsRunning = false;
                if (mode == 4)
                {
                    NetworkServer.DisconnectAllConnections();
                    NetworkServer.Shutdown();
                    NetworkClient.Shutdown();
                }

                SceneManager.LoadScene(2);
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        TimeCount.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void PauseMenu()
    {
        Time.timeScale = 0;
        pauseMenu.gameObject.SetActive(true);
        pauseBtn.gameObject.SetActive(false);
    }

    private void Continue()
    {
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    private void DisconnectServer()
    {
        NetworkClient.Disconnect();
        NetworkServer.DisconnectAll();
        SceneManager.LoadScene("MainMenu");
        
    }
}
