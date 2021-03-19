using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class TurnMenu : NetworkBehaviour
{
    
    public Text Youturn;
    public Image panel;
    private TurnCounter turnCounter;
    private SkillSystemOnline skillSystem;
    private int redCD, blueCD;


    // Start is called before the first frame update
    void Start()
    {
        turnCounter = FindObjectOfType<TurnCounter>().GetComponent<TurnCounter>();
        skillSystem = FindObjectOfType<SkillSystemOnline>().GetComponent<SkillSystemOnline>();
        redCD = PlayerPrefs.GetInt("RedCD", 0);
        blueCD = PlayerPrefs.GetInt("BlueCD", 0);
    }

    public void getDestroy()
    {
        Destroy(panel);
        Destroy(Youturn);
    }

    private void Update()
    {

        if (turnCounter == null)
        {
            try
            {
                turnCounter = FindObjectOfType<TurnCounter>().GetComponent<TurnCounter>();
            }
            catch(Exception ex)
            {

            }
        }
        else
        {
            if (turnCounter.turnMenu == false)
            {
                Youturn.gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
                Debug.Log($"TurnDisplay : Debug - Update : {turnCounter.turnMenu}");
            }
            else
            {
                Youturn.gameObject.SetActive(true);
                panel.gameObject.SetActive(true);
                Debug.Log($"TurnDisplay : Debug - Update : {turnCounter.turnMenu}");
            }
        }
        
    }

    public IEnumerator StartDisplay()
    {
        yield return new WaitForSeconds(1.5f);
        turnCounter.TurnDisplay(false);
        Debug.Log($"TurnDisplay : Debug - StartDisplay {turnCounter.turnMenu}");
    }

    public IEnumerator YourturnMenu(int turn)
    {
        try
        {
            Debug.Log($"TurnDisplay : Debug - Start Setting {turnCounter.turnMenu}");
            //Check Blue or Red team Turn, change text
            //0 is Red turn , 1 is Blue turn.
            //Go Next for later

            if (turn == 0)
            {
                try
                {
                    if (redCD != skillSystem.redCD)
                    {
                        if (skillSystem.tempRedCD != skillSystem.redCD || skillSystem.tempRedCD == 0)
                        {
                            redCD = skillSystem.redCD;
                            if (redCD > 0)
                            {
                                int temp = redCD - 1;
                                skillSystem.CmdSetTempCD(false, temp);
                                skillSystem.CmdSetCD(false, temp);

                            }
                            if (skillSystem.tempBlueCD > 0)
                            {
                                int temp = skillSystem.tempBlueCD - 1;
                                skillSystem.CmdSetTempCD(true, temp);
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    //Debug
                }
                
                Youturn.GetComponent<Text>().color = Color.red;
                Youturn.text = "Red Team is now your Turn";
                Youturn.gameObject.SetActive(true);
                panel.gameObject.SetActive(true);
            }
            else if (turn == 1)
            {
                try
                {
                    if (blueCD != skillSystem.blueCD)
                    {
                        if (skillSystem.tempBlueCD != skillSystem.blueCD || skillSystem.tempBlueCD == 0)
                        {
                            blueCD = skillSystem.blueCD;
                            if (blueCD > 0)
                            {
                                int temp = blueCD - 1;
                                skillSystem.CmdSetTempCD(true, temp);
                                skillSystem.CmdSetCD(true, temp);

                            }
                            if (skillSystem.tempRedCD > 0)
                            {
                                int temp = skillSystem.tempRedCD - 1;
                                skillSystem.CmdSetTempCD(false, temp);
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    //Debug
                }
                
                
                Youturn.GetComponent<Text>().color = Color.blue;
                Youturn.text = "Blue Team is now your Turn";
                Youturn.gameObject.SetActive(true);
                panel.gameObject.SetActive(true);
            }
            turnCounter.TurnDisplay(true);
            Debug.Log($"TurnDisplay : Debug - SetTurn {turnCounter.turnMenu}");
        }
        catch
        {

        }


        yield return new WaitForSeconds(1.5f);
        try
        {
            turnCounter.TurnDisplay(false);
            Debug.Log($"TurnDisplay : Debug - SetTurn {turnCounter.turnMenu}");
        }
        catch (Exception e)
        {
            //Avoid crash
        }
        turnCounter.DragShot(false);

    }


}
