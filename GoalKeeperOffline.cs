using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeeperOffline : MonoBehaviour
{
    // End Position for Player and Enemy GoalKeeper to use Lerp with PingPong to automatically move to defence.
    Vector3 endPosPlayer = new Vector3(1.84f, -1.787207f, 49.4f);
    Vector3 endPosEnemy = new Vector3(2.7f, -1.787207f, 49.4f);
    //Vector3 PlayerStartPos = new Vector3(-6.6f, -1.787207f, 49.4f);
    //Vector3 EnemyStartPos = new Vector3(-6.4f, -1.787207f,49.4f);
    public Vector3 startPos = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {

        startPos = gameObject.transform.localPosition;

    }
    // Update is called once per frame
    void Update()
    {
        RunMovement();

    }

    void RunMovement()
    {
        if(startPos != Vector3.zero)
        {
            if(PlayerPrefs.GetInt("Mode", 0) != 5)
            {
                if (gameObject.tag == "GoalKeeperPlayer")
                {
                    gameObject.transform.localPosition = Vector3.Lerp(startPos, new Vector3(startPos.x + 9f, startPos.y, startPos.z), Mathf.PingPong(Time.time, 1));
                }
                else if (gameObject.tag == "GoalKeeperEnemy")
                {
                    gameObject.transform.localPosition = Vector3.Lerp(startPos, new Vector3(startPos.x + 9f, startPos.y, startPos.z), Mathf.PingPong(Time.time, 1));
                }
            }
            else if(PlayerPrefs.GetInt("Mode", 0) == 5 && Server.isServer)
            {
                if (gameObject.tag == "GoalKeeperPlayer")
                {
                    gameObject.transform.localPosition = Vector3.Lerp(startPos, new Vector3(startPos.x + 9f, startPos.y, startPos.z), Mathf.PingPong(Time.time, 1));
                }
                else if(gameObject.tag == "GoalKeeperEnemy")
                {
                    gameObject.transform.localPosition = Vector3.Lerp(startPos, new Vector3(startPos.x - 9f, startPos.y, startPos.z), Mathf.PingPong(Time.time, 1));
                }
            }    
        }
    }
}
