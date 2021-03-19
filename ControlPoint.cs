using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPoint : MonoBehaviour
{
    public Rigidbody player;
    public LineRenderer line;
    private float lineLength;
    private ColorSystem change;
    public Gradient blueTeamColor;
    public Gradient redTeamColor;

    // Update is called once per frame
    void Update()
    {
        try
        {
            change = FindObjectOfType<ColorSystem>().GetComponent<ColorSystem>();
            player = change.newSelect.gameObject.GetComponent<Rigidbody>();
            this.gameObject.transform.position = player.position;
            //If Clicked mouse button
            if (Input.GetMouseButton(0))
            {
                //Get mouse click start position . If it is (0,0,0) . Don't show line .
                //Need to click on the object to drag and release.
                if (change.newSelect.tag == "Player" || change.newSelect.tag == "Enemy")
                {
                    if (change.newSelect.tag == "Player")
                        line.gameObject.GetComponent<LineRenderer>().colorGradient = blueTeamColor;
                    else if (change.newSelect.tag == "Enemy")
                        line.gameObject.GetComponent<LineRenderer>().colorGradient = redTeamColor;

                    MousePosition();
                    line.gameObject.SetActive(true);
                    line.SetPosition(0, transform.position);
                    //Maximum the Line Length from 5 to 27 to avoid too short or too long.
                    line.SetPosition(1, transform.position + transform.forward * Mathf.Clamp(lineLength, 5, 20));
                    //Debug.LogWarning("Line Length : " + lineLength);
                }
                else
                {

                    line.gameObject.SetActive(false);
                    return;
                }
            }

        }
        catch
        {
            //Debug uses
        }
        
    }
    private void MousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, 1,0));
        float distance;
        if (plane.Raycast(ray, out distance))
        {
            Vector3 target = ray.GetPoint(distance);
            Vector3 direction = target - transform.position;
            lineLength = target.magnitude - transform.position.magnitude;
            float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            this.gameObject.transform.rotation = Quaternion.Euler(0 , rotation ,0);
        }
    }

    

}
