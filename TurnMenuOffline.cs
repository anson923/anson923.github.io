using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurnMenuOffline : MonoBehaviour
{
    public Text Youturn;
    public Image panel,blueTurnHUD,redTurnHUD;
    private DragSystemOffline dragSystem;
    private int redCD, blueCD, tempRedCD, tempBlueCD;
    //Color Red Turn = 0 , Blue turn = 1;
    public static int currentTurn = 1;
    // Start is called before the first frame update
    void Start()
    {
        //Get Component
        dragSystem = FindObjectOfType<DragSystemOffline>().GetComponent<DragSystemOffline>();
        
        if (PlayerPrefs.GetInt("Mode", 0) == 1 || PlayerPrefs.GetInt("Mode", 0) == 5)
        {
            if (PlayerPrefs.GetInt("Mode", 0) == 5)
            {
                currentTurn = PlayerPrefs.GetInt("Turn", 1);
            }
            StartCoroutine(YourturnMenu());
        }
        else if (PlayerPrefs.GetInt("Mode", 0) == 2 || PlayerPrefs.GetInt("Mode", 0) == 3)
        {
            try
            {
                Youturn.gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
            }
            catch (Exception e)
            {
                //Avoid crash
            }
        }

        redCD = PlayerPrefs.GetInt("RedCD", 0);
        blueCD = PlayerPrefs.GetInt("BlueCD", 0);
        tempRedCD = 0;
        tempBlueCD = 0;
    }

    private void Update()
    {
        int CurrentTurn = 0 ;
        if (PlayerPrefs.GetInt("Mode", 0) == 5)
        {
            CurrentTurn = currentTurn;
        }
        else
        {
            CurrentTurn = dragSystem.CurrentTurn;
        }
        if (blueTurnHUD != null && redTurnHUD != null)
        {
            //Color Red Turn = 0 , Blue turn = 1;
            if (CurrentTurn == 1)
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
    }

    public void getDestroy()
    {
        Destroy(panel);
        Destroy(Youturn);
    }

    public IEnumerator YourturnMenu()
    {
        try
        {
            int CurrentTurn = 0;
            if (PlayerPrefs.GetInt("Mode", 0) == 5)
            {
                CurrentTurn = currentTurn;

            }
            else
            {
                CurrentTurn = dragSystem.CurrentTurn;
            }
            //Check Blue or Red team Turn, change text
            //0 is Red turn , 1 is Blue turn.
            //Go Next for later

            if (CurrentTurn == 0)
            {

                Youturn.GetComponent<Text>().color = Color.red;
                Youturn.text = "Red Team is now your Turn";
                if (redCD != PlayerPrefs.GetInt("RedCD", 0))
                {
                    if (tempRedCD != PlayerPrefs.GetInt("RedCD", 0) || tempRedCD == 0)
                    {
                        redCD = PlayerPrefs.GetInt("RedCD", 0);
                        if (redCD > 0)
                        {
                            tempRedCD = redCD - 1;
                            PlayerPrefs.SetInt("RedCD", tempRedCD);
                        }
                        if (tempBlueCD > 0)
                        {
                            tempBlueCD -= -1;
                        }
                    }
                }
            }
            else if (CurrentTurn == 1)
            {

                Youturn.GetComponent<Text>().color = Color.blue;
                Youturn.text = "Blue Team is now your Turn";

                if (blueCD != PlayerPrefs.GetInt("BlueCD", 0))
                {
                    if (tempBlueCD != PlayerPrefs.GetInt("BlueCD", 0) || tempBlueCD == 0)
                    {
                        blueCD = PlayerPrefs.GetInt("BlueCD", 0);
                        if (blueCD > 0)
                        {
                            tempBlueCD = blueCD - 1;
                            PlayerPrefs.SetInt("BlueCD", tempBlueCD);
                        }
                        if (tempRedCD > 0)
                        {
                            tempRedCD -= -1;
                        }
                    }
                }
            }

            Youturn.gameObject.SetActive(true);
            panel.gameObject.SetActive(true);

        }
        catch
        {

        }


        yield return new WaitForSeconds(1.5f);
        try
        {
            Youturn.gameObject.SetActive(false);
            panel.gameObject.SetActive(false);
        }
        catch (Exception e)
        {
            //Avoid crash
        }

        
    }

}
