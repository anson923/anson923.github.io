using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror
{
    public class Network_Manager_FootballGame : NetworkManager
    {

        // Custom NetworkManager that simply assigns the correct racket positions when
        // spawning players. The built in RoundRobin spawn method wouldn't work after
        // someone reconnects (both players would be on the same side).
        [AddComponentMenu("")]

        //public Transform leftRacketSpawn;
        //public Transform rightRacketSpawn;
        GameObject ball;
        GameObject player;

        public override void OnServerAddPlayer(NetworkConnection conn)
        {

            // add player at correct spawn position
            //Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
            Debug.Log("Server adding player...");
            if (numPlayers == 0)
            {
                foreach (GameObject obj in spawnPrefabs)
                {
                    if (obj.tag == "Player")
                    {
                        player = Instantiate(obj);
                        NetworkServer.Spawn(player, conn);
                        NetworkServer.AddPlayerForConnection(conn, player);
                    }

                }
                foreach (GameObject obj in spawnPrefabs)
                {
                    if (obj.tag == "GoalKeeperPlayer")
                    {
                        player = Instantiate(obj);
                        NetworkServer.Spawn(player, conn);
                        NetworkServer.AddPlayerForConnection(conn, player);
                    }

                }
                
            }
            else if (numPlayers == 1)
            {
                foreach (GameObject obj in spawnPrefabs)
                {
                    if (obj.tag == "Enemy")
                    {
                        player = Instantiate(obj);
                        NetworkServer.Spawn(player, conn);
                        NetworkServer.AddPlayerForConnection(conn, player);
                    }

                }
                foreach (GameObject obj in spawnPrefabs)
                {
                    if (obj.tag == "GoalKeeperEnemy")
                    {
                        player = Instantiate(obj);
                        NetworkServer.Spawn(player, conn);
                        NetworkServer.AddPlayerForConnection(conn, player);
                    }

                }
                
            }

            //if(numPlayers == 0)
            //{
            //    player = Instantiate(playerPrefab, start.position, start.rotation);
            //    NetworkServer.AddPlayerForConnection(conn, player);


            //}
            //else if(numPlayers == 1)
            //{
            //    player = Instantiate(EnemyPrefab, start.position, start.rotation);
            //    NetworkServer.AddPlayerForConnection(conn, player);
            //}

            // spawn ball if reach maximum players
            //1 == 1 player , 2 == 2 players
            if (numPlayers == 1)
            {
                ball = Instantiate(spawnPrefabs.Find(prefab => prefab.tag == "Ball"));
                NetworkServer.Spawn(ball);
            }
        }

        private void DisconnectAll()
        {
            NetworkServer.Shutdown();
            NetworkClient.Shutdown();
            StopServer();
            if (PlayerPrefs.GetFloat("Timer", 0) != 0f)
                SceneManager.LoadScene("Disconnected");
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            // destroy ball
            if (ball != null)
                NetworkServer.Destroy(ball);

            // call base functionality (actually destroys the player)
            DisconnectAll();
            base.OnServerDisconnect(conn);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            DisconnectAll();
            base.OnClientDisconnect(conn);
        }

        
    }
}
