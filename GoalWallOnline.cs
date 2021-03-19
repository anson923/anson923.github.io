using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalWallOnline : MonoBehaviour
{
    private GoalTextOnline goalText;
    private TurnCounter counter;
    private string teamName, message;
    private SoccerSpeedControl soccer;

    private void Start()
    {
        try
        {

            goalText = FindObjectOfType<GoalTextOnline>().GetComponent<GoalTextOnline>();
            counter = FindObjectOfType<TurnCounter>().GetComponent<TurnCounter>();
            soccer = FindObjectOfType<SoccerSpeedControl>().GetComponent<SoccerSpeedControl>();
        }
        catch (Exception e)
        {
            //Avoid crash
        }
        //Debug data remove
        //PlayerPrefs.DeleteAll();
    }

    private void Update()
    {
        if(goalText == null || counter == null || soccer == null)
        {
            try
            {

                goalText = FindObjectOfType<GoalTextOnline>().GetComponent<GoalTextOnline>();
                counter = FindObjectOfType<TurnCounter>().GetComponent<TurnCounter>();
                soccer = FindObjectOfType<SoccerSpeedControl>().GetComponent<SoccerSpeedControl>();
            }
            catch (Exception e)
            {
                //Avoid crash
            }
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        #region Collide Event for Ball
        //If it collide with the ball
        if (collision.gameObject.CompareTag("Ball"))
        {
            
            //Avoid ball bounce back tigger second time
            goalText.CmdSoccerPause();
            //For Team2 Side(Right)
            if (gameObject.tag == "GoalLineRight")
            {
                //If player is Team1 player
                if (soccer.lastTouch.layer == 9)
                {
                    //Score a goal and add value
                    counter.AddScore(true, 1);
                    teamName = "Team Blue Score a";
                    message = "GOAL!";
                }
                else
                {
                    //Score an own goal and add value
                    counter.AddScore(true, 1);
                    teamName = "Team Red Score an";
                    message = "OWN GOAL!";
                }
                
            }
            //For Team1 Side(Left)
            else if (gameObject.tag == "GoalLineLeft")
            {
                //If player is Team2 player
                if (soccer.lastTouch.layer == 10)
                {
                    //Score a goal and add value
                    counter.AddScore(false, 1);
                    teamName = "Team Red Score a";
                    message = "GOAL!";
                }
                else
                {
                    //Score an own goal and add value
                    counter.AddScore(false, 1);
                    teamName = "Team Blue Score an";
                    message = "OWN GOAL!";
                }
            }
            //Show Goal message
            goalText.OnGoal(teamName, message);

            //Add score , reset player position . Other Player's turn
            Debug.Log("Ball hit the Goal Line!");

        }
        #endregion
       
    }

}
