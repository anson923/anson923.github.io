using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GoalWallBP : MonoBehaviour
{
    private GoalText goalText;
    private int Team1Score , Team2Score;
    private string teamName, message;
    private TutorialStage tutorial;
    private TurnMenuOffline turnMenu;

    private void Start()
    {
        try
        {
            
            goalText = FindObjectOfType<GoalText>().GetComponent<GoalText>();
            tutorial = FindObjectOfType<TutorialStage>().GetComponent<TutorialStage>();
        }
        catch(Exception e)
        {
            //Avoid crash
        }
        //Debug data remove
        //PlayerPrefs.DeleteAll();
    }

    private void Update()
    {
        if(turnMenu == null)
        {
            try
            {
                turnMenu = FindObjectOfType<TurnMenuOffline>().GetComponent<TurnMenuOffline>();
            }
            catch(Exception ex)
            {
                //Debug.
            }
        }
        
    }


    private void OnTriggerEnter(Collider collision)
    {
        #region Collide Event for Ball
        //If it collide with the ball
        try
        {
            if (collision.gameObject.CompareTag("Ball") && PlayerPrefs.GetInt("Mode", 0) == 1 || PlayerPrefs.GetInt("Mode", 0) == 5)
            {
                Physics.IgnoreCollision(collision, this.gameObject.GetComponent<Collider>());
                turnMenu.enabled = false;
                //For Team2 Side(Right)
                if (gameObject.tag == "GoalLineRight")
                {
                    //If player is Team1 player
                    if (collision.gameObject.GetComponent<SoccerSpeedControlOffline>().lastTouch.layer == 9)
                    {
                        //Score a goal and add value
                        Team1Score = int.Parse(goalText.team1ScoreText.text);
                        Team1Score += 1;
                        PlayerPrefs.SetInt("Team1", Team1Score);
                        teamName = "Team Blue Score a";
                        message = "GOAL!";
                    }
                    else
                    {
                        //Score an own goal and add value
                        Team1Score = int.Parse(goalText.team1ScoreText.text);
                        Team1Score += 1;
                        PlayerPrefs.SetInt("Team1", Team1Score);
                        teamName = "Team Red Score an";
                        message = "OWN GOAL!";
                    }
                }
                //For Team1 Side(Left)
                else if (gameObject.tag == "GoalLineLeft")
                {
                    //If player is Team2 player
                    if (collision.gameObject.GetComponent<SoccerSpeedControlOffline>().lastTouch.layer == 10)
                    {
                        //Score a goal and add value
                        Team2Score = int.Parse(goalText.team2ScoreText.text);
                        Team2Score += 1;
                        PlayerPrefs.SetInt("Team2", Team2Score);
                        teamName = "Team Red Score a";
                        message = "GOAL!";
                    }
                    else
                    {
                        //Score an own goal and add value
                        Team2Score = int.Parse(goalText.team2ScoreText.text);
                        Team2Score += 1;
                        PlayerPrefs.SetInt("Team2", Team2Score);
                        teamName = "Team Blue Score an";
                        message = "OWN GOAL!";
                    }
                }
                //Show Goal message
                StartCoroutine(goalText.OnGoal(teamName, message));

                //Add score , reset player position . Other Player's turn
                Debug.Log("Ball hit the Goal Line!");
            }

            else if (collision.gameObject.CompareTag("Ball") && PlayerPrefs.GetInt("Mode", 0) == 2)
            {
                teamName = "Team Blue Score a";
                message = "GOAL!";
                //Show Goal message
                StartCoroutine(goalText.OnGoal(teamName, message));
                Debug.Log("Ball hit the Goal Line!");
                tutorial.log6 = true;
            }
            #endregion
            Physics.IgnoreCollision(collision, this.gameObject.GetComponent<Collider>(), false);
        }
        catch(Exception ex)
        {
            //Debug.
        }
       
    }

}
