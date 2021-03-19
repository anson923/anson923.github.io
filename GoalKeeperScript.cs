using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GoalKeeperScript : NetworkBehaviour
{
    // End Position for Player and Enemy GoalKeeper to use Lerp with PingPong to automatically move to defence.
    Vector3 startPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {

        startPos = gameObject.transform.localPosition;
        
    }
    // Update is called once per frame
    void Update()
    {
        Movement();

    }

    void Movement()
    {
        CmdMovement();
    }

    void RunMovement()
    {
        Vector3 endPostemp = new Vector3(startPos.x + 9f, startPos.y, startPos.z);
        gameObject.transform.localPosition = Vector3.Lerp(startPos, endPostemp, Mathf.PingPong(Time.time, 1));

    }

    [Command(ignoreAuthority = true)]
    void CmdMovement()
    {
        RunMovement();
    }


}
