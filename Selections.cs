using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;

public class Selections : NetworkBehaviour
{
    public ColorSystem pullRelease;
    private Vector3 originPosition;
    public TurnCounter turnCounter;
    // Start is called before the first frame update
    void Start()
    {
        pullRelease = FindObjectOfType<ColorSystem>().GetComponent<ColorSystem>();
        originPosition = transform.localPosition;
        turnCounter = FindObjectOfType<TurnCounter>().GetComponent<TurnCounter>();

    }

    public Vector3 getOrigin
    {
        get { return originPosition; }
    }
    // Update is called once per frame
    void Update()
    {
        //Keep updating moving object.
        if (this.gameObject.GetComponent<Rigidbody>().velocity != Vector3.zero)
        {
            //pullRelease.movingObject = this.gameObject;
        }
        if (turnCounter == null)
        {
            try
            {
                turnCounter = FindObjectOfType<TurnCounter>().GetComponent<TurnCounter>();
            }
            catch (Exception ex)
            {
                //Debug.
            }
        }
    }

    //When Mouse Clicked On player
    private void OnMouseOver()
    {

        //If tag is player or enemy
        if (gameObject.tag == "Player" || gameObject.tag == "Enemy")
        {

            //If press click
            if (Input.GetMouseButtonDown(0))
            {
                if (PlayerPrefs.GetInt("Mode", 0) == 4)
                {
                    pullRelease.movingObject = gameObject;

                    turnCounter = FindObjectOfType<TurnCounter>().GetComponent<TurnCounter>();
                    //Check Parent isLocalPlayer?

                    if (gameObject.tag == "Player" && isServer && isClient)
                    {


                        if (turnCounter.currentTurn == 1 && gameObject.tag == "Player")
                        {
                            pullRelease.newSelect = gameObject;

                        }

                        //Call Change Color function
                        pullRelease.ChangeColors(true, turnCounter.currentTurn);
                        return;
                    }
                    else if (gameObject.tag == "Enemy" && isClientOnly)
                    {
                        if (turnCounter.currentTurn == 0 && gameObject.tag == "Enemy")
                        {
                            pullRelease.newSelect = gameObject;
                            Debug.Log("Newselect");
                        }
                        //Call Change Color function
                        pullRelease.ChangeColors(true, turnCounter.currentTurn);
                        return;
                    }

                }

                //Call Change Color function
                pullRelease.ChangeColors(false, turnCounter.currentTurn);

            }
        }
    }


}


