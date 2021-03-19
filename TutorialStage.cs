using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialStage : MonoBehaviour
{
    private int clicked = 0;
    public List<GameObject> textGroup = new List<GameObject>();
    public List<GameObject> players = new List<GameObject>();
    private bool skillCkicked = false;
    public bool log6 = false;
    private bool setPos = false;
    private bool soccerSet = false;
    private bool moved = false;
    private bool touched = false;
    public Button continueBtn;
    public Button pauseBtn;
    public Button continueGamebtn;
    public Button mainMenuBtn;
    public Button skillBtn;
    private ColorSystemOffline colorSystem;
    public GameObject line;
    public GameObject player;
    public GameObject soccer;
    public GameObject GoalKeeper;
    public Text skillText;
    public Image panel;
    public Image skillPanel;

    private Vector3 playerPenaltyPos = new Vector3(-2.1f, -1.787207f, -60f);
    private Vector3 soccerPos = new Vector3(2.52f, -4.073277f, -34.3f);

    // Start is called before the first frame update
    void Start()
    {
        colorSystem = FindObjectOfType<ColorSystemOffline>(); GetComponent<ColorSystemOffline>();


        continueBtn.onClick.AddListener(ClickedContinue);
        pauseBtn.onClick.AddListener(PauseMenu);
        continueGamebtn.onClick.AddListener(Continue);
        mainMenuBtn.onClick.AddListener(MainMenu);
        GoalKeeper.gameObject.SetActive(false);
        soccer.gameObject.SetActive(false);
        panel.gameObject.SetActive(false);
        if (PlayerPrefs.GetInt("Mode", 0) == 3)
        {
            skillText.text = "1";
            skillPanel.gameObject.SetActive(true);
            skillBtn.interactable = false;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
            {
                players.Add(obj.gameObject);
            }
            skillBtn.onClick.AddListener(BlueTeamReset);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetInt("Mode", 0) == 2)
        {
            if ((clicked == 3 || clicked == 6 || clicked == 9))
            {
                colorSystem.okToMove = true;
                continueBtn.gameObject.SetActive(false);
                line.SetActive(true);
                if (clicked == 6 || clicked == 9)
                {
                    if (setPos == false)
                    {
                        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        player.GetComponent<Transform>().localPosition = playerPenaltyPos;
                        setPos = true;
                    }
                    soccer.gameObject.SetActive(true);
                    if (clicked == 9)
                    {
                        if (soccerSet == false)
                        {
                            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            soccer.GetComponent<Transform>().localPosition = soccerPos;
                            soccerSet = true;
                        }

                        GoalKeeper.gameObject.SetActive(true);
                    }
                    if (player.GetComponent<Rigidbody>().velocity.magnitude >= 8f)
                    {
                        moved = true;
                    }

                    if (moved == true && touched == false && player.GetComponent<Rigidbody>().velocity.magnitude == 0 && soccer.GetComponent<Rigidbody>().velocity.magnitude == 0)
                    {
                        if (log6 == false)
                        {
                            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            player.GetComponent<Transform>().localPosition = playerPenaltyPos;
                            setPos = true;
                            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            soccer.GetComponent<Transform>().localPosition = soccerPos;
                        }


                    }
                }

                if (clicked == 3 && player.GetComponent<Rigidbody>().velocity.magnitude >= 8f)
                {
                    clicked++;
                    continueBtn.gameObject.SetActive(true);
                    colorSystem.okToMove = false;
                }
                else if (clicked == 6 && log6)
                {
                    clicked++;
                    continueBtn.gameObject.SetActive(true);
                    colorSystem.okToMove = false;
                    log6 = false;
                }

                else if (clicked == 9 && log6)
                {
                    clicked++;
                    continueBtn.gameObject.SetActive(true);
                    colorSystem.okToMove = false;
                    log6 = false;
                }
            }
            else
            {
                setPos = false;
                soccerSet = false;
                colorSystem.okToMove = false;
                continueBtn.gameObject.SetActive(true);
                line.SetActive(false);
            }
        }

        //MODE 3
        if(PlayerPrefs.GetInt("Mode", 0) == 3)
        {
            if ((clicked == 6 || clicked == 9))
            {
                line.SetActive(true);
                colorSystem.okToMove = true;
                if (clicked == 9)
                {
                    colorSystem.okToMove = false;
                    skillBtn.interactable = true;
                }


                continueBtn.gameObject.SetActive(false);
                if (clicked == 6 && player.GetComponent<Rigidbody>().velocity.magnitude >= 8f)
                {
                    clicked++;
                    continueBtn.gameObject.SetActive(true);
                    colorSystem.okToMove = false;
                    skillPanel.gameObject.SetActive(false);
                }
                if (clicked == 9 && skillCkicked == true)
                {
                    clicked++;
                    continueBtn.gameObject.SetActive(false);
                    skillPanel.gameObject.SetActive(true);
                    skillText.text = "5";
                    skillBtn.interactable = false;
                }
            }
            else
            {
                colorSystem.okToMove = false;
                continueBtn.gameObject.SetActive(true);
                line.SetActive(false);
            }
        }


        if (clicked == textGroup.Count)
        {
            SceneManager.LoadScene("MainMenu");
        }
        for (int i = 0; i < textGroup.Count; i++)
        {
            if (i == clicked)
            {
                textGroup[i].SetActive(true);
            }
            else
            {
                textGroup[i].SetActive(false);
            }
        }

        


    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            touched = true;
        }
    }

    private void ClickedContinue()
    {
        clicked++;
    }

    private void PauseMenu()
    {
        Time.timeScale = 0;
        panel.gameObject.SetActive(true);
        pauseBtn.gameObject.SetActive(false);
    }

    private void Continue()
    {
        Time.timeScale = 1;
        panel.gameObject.SetActive(false);
        pauseBtn.gameObject.SetActive(true);
    }

    private void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    private void BlueTeamReset()
    {
        try
        {
            player.GetComponent<Transform>().localPosition = player.GetComponent<SelectionOffline>().getOrigin;
            skillCkicked = true;

        }
        catch (Exception e)
        {
            //Bugs?
            //Debug.Log(e);
        }
    }
}
