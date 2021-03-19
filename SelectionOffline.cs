using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionOffline : MonoBehaviour
{
    private ColorSystemOffline ColorSystem;
    private DragSystemOffline dragSystem;
    private Vector3 originPosition;
    // Start is called before the first frame update
    void Start()
    {
        dragSystem = FindObjectOfType<DragSystemOffline>().GetComponent<DragSystemOffline>();
        ColorSystem = FindObjectOfType<ColorSystemOffline>().GetComponent<ColorSystemOffline>();
        originPosition = gameObject.GetComponent<Transform>().localPosition;
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
            ColorSystem.movingObject = this.gameObject;
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
                ColorSystem.movingObject = this.gameObject;
                //Assign this object to newSelect
                if (PlayerPrefs.GetInt("Mode", 0) == 1)
                {
                    if (dragSystem.CurrentTurn == 1 && gameObject.tag == "Player")
                        ColorSystem.newSelect = gameObject.gameObject;
                    else if (dragSystem.CurrentTurn == 0 && gameObject.tag == "Enemy")
                        ColorSystem.newSelect = gameObject.gameObject;
                }
                else if (PlayerPrefs.GetInt("Mode", 0) == 2 || PlayerPrefs.GetInt("Mode", 0) == 3)
                {
                    if (gameObject.tag == "Player")
                        ColorSystem.newSelect = gameObject.gameObject;
                }
                else if(PlayerPrefs.GetInt("Mode", 0) == 5)
                {
                    if (TurnMenuOffline.currentTurn == 1 && gameObject.tag == "Player" && Server.isServer)
                        ColorSystem.newSelect = gameObject.gameObject;
                    else if (TurnMenuOffline.currentTurn == 0 && gameObject.tag == "Enemy" && Server.isServer == false)
                        ColorSystem.newSelect = gameObject.gameObject;


                }

                //Call Change Color function
                ColorSystem.ChangeColors();

            }
        }

    }
}

