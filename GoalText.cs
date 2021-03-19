using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoalText : MonoBehaviour
{
    public Text scoreText1;
    public Text scoreText2;
    public Text team1ScoreText;
    public Text team2ScoreText;
    private float delayTime = 1.5f;


    // Start is called before the first frame update
    void Start()
    {
        scoreText1.gameObject.SetActive(false);
        scoreText2.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        team1ScoreText.text = PlayerPrefs.GetInt("Team1", 0).ToString();
        team2ScoreText.text = PlayerPrefs.GetInt("Team2", 0).ToString();
    }

    public IEnumerator OnGoal(string team , string message)
    {
        scoreText1.text = team;
        scoreText2.text = message;
        scoreText1.gameObject.SetActive(true);
        scoreText2.gameObject.SetActive(true);
        yield return new WaitForSeconds(delayTime);
        scoreText1.gameObject.SetActive(false);
        scoreText2.gameObject.SetActive(false);
        if (PlayerPrefs.GetInt("Mode",0) == 1)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        else if(PlayerPrefs.GetInt("Mode", 0) == 2 || PlayerPrefs.GetInt("Mode", 0) == 3)
        {
            scoreText1.gameObject.SetActive(false);
            scoreText2.gameObject.SetActive(false);
        }
        else if (PlayerPrefs.GetInt("Mode", 0) == 5)
        {
            if(Server.isServer)
            {
                Server.goal = true;
            }
            else
            {
                Client.goal = true;
            }
        }
        
    }
}
