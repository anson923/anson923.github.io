using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Mirror
{
    public class NetworkHUDManager : NetworkBehaviour
    {
        public InputField networkIP;
        public Text ConnectedText;
        public Button hostBtn;
        public Button connectBtn;
        public Button cancelHost;
        public Button cancelConnectionBtn;
        public Canvas lobby;
        public Image turnPanel;
        public NetworkManager network;
        public GameManager networkAuth;
        public TurnCounter counter;

        public void Awake()
        {
            networkIP.text = "localhost";
            if (cancelHost != null)
            {
                cancelHost.onClick.AddListener(onCancelHostClicked);
                cancelHost.gameObject.SetActive(false);
            }
            if (connectBtn != null)
            {
                connectBtn.onClick.AddListener(onConnectClicked);
            }
            if(cancelConnectionBtn != null)
            {
                cancelConnectionBtn.onClick.AddListener(onCancelConntectionClicked);
                cancelConnectionBtn.gameObject.SetActive(false);
            }

            ConnectedText.gameObject.SetActive(false);
            lobby.gameObject.SetActive(true);
        }


        // Update is called once per frame
        public void Update()
        {
            if (counter != null)
            {
                if (counter.teamBlueScores == 3 || counter.teamRedScores == 3)
                {
                    //True for blue team, False for red team
                    if (counter.teamBlueScores == 3)
                        CmdDisconnectAll(true);
                    else
                        CmdDisconnectAll(false);
                }
            }
            if (counter == null)
            {
                try
                {
                    counter = FindObjectOfType<TurnCounter>().GetComponent<TurnCounter>();
                }
                catch(Exception ex)
                {
                    //Debug
                }
            }
            if (lobby != null)
            {
                if (lobby.gameObject.activeInHierarchy == true)
                {
                    if (networkAuth.gameObject.GetComponent<NetworkIdentity>().isServer)
                    {
                        if (network.numPlayers != network.maxConnections)
                        {
                            ConnectedText.text = $"Connected to the server {networkIP.text} \n Current Player: {network.numPlayers} / {NetworkServer.maxConnections}";
                        }
                        //Server-side MaxConnection action
                        else
                        {
                            ConnectedText.text = $"Connected to the server {networkIP.text} \n Current Player: {network.numPlayers} / {NetworkServer.maxConnections}";
                            lobby.gameObject.SetActive(false);
                            turnPanel.gameObject.SetActive(true);
                            StartCoroutine(networkAuth.GetComponent<TurnMenu>().StartDisplay());
                        }
                    }
                    //Client-side MaxConnection action
                    else if (NetworkClient.isConnected && NetworkClient.active)
                    {

                        ConnectedText.text = $"Connected to the server {networkIP.text} \n Current Player: {NetworkServer.maxConnections} / {NetworkServer.maxConnections}";
                        ConnectedText.gameObject.SetActive(false);
                        cancelHost.gameObject.SetActive(false);
                        lobby.gameObject.SetActive(false);
                        turnPanel.gameObject.SetActive(true);
                    }
                }

                
                
            }  
        }

        void CmdDisconnectAll(bool team)
        {
            RpcDisconnectAll(team);
        }

        void RpcDisconnectAll(bool team)
        {
            if(team)
            {
                PlayerPrefs.SetInt("Team1", 3);
                PlayerPrefs.SetInt("Team2", 0);
            }
            else
            {
                PlayerPrefs.SetInt("Team1", 0);
                PlayerPrefs.SetInt("Team2", 3);
            }
            Destroy(GameObject.Find("NetworkManager"));
            Destroy(GameObject.Find("Counter"));
            NetworkServer.DisconnectAll();
            NetworkClient.Disconnect();
            NetworkManager.Shutdown();
            SceneManager.LoadScene("GameWinScene");
        }


        public void onHostClicked()
        {

            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (networkIP.text == null || networkIP.text != "localhost")
                        network.networkAddress = networkIP.text;

                    network.StartHost();
                    
                }
                ConnectedText.gameObject.SetActive(true);
                ConnectedText.text = $"Connecting to {network.networkAddress}";
                networkIP.gameObject.SetActive(false);
                hostBtn.gameObject.SetActive(false);
                cancelHost.gameObject.SetActive(true);
                connectBtn.gameObject.SetActive(false);
                PlayerPrefs.SetInt("Server", 1);

            }

        }

        private void onConnectClicked()
        {
            if (!NetworkClient.active)
            {
                // Client + IP
                network.networkAddress = networkIP.text;
                network.StartClient();
                connectBtn.gameObject.SetActive(false);
                networkIP.gameObject.SetActive(false);
                hostBtn.gameObject.SetActive(false);
                connectBtn.gameObject.SetActive(false);
                ConnectedText.gameObject.SetActive(true);
                ConnectedText.text = $"Waiting to connect to the server {networkIP.text} ...";
                cancelConnectionBtn.gameObject.SetActive(true);
                PlayerPrefs.SetInt("Server", 0);
            }
        }

        private void onCancelHostClicked()
        {
            // stop host if host mode
            if (NetworkServer.active && NetworkClient.isConnected)
            {

                network.StopHost();
                ConnectedText.gameObject.SetActive(false);
                networkIP.gameObject.SetActive(true);
                hostBtn.gameObject.SetActive(true);
                cancelHost.gameObject.SetActive(false);
                connectBtn.gameObject.SetActive(true);
            }
        }


        private void onCancelConntectionClicked()
        {
            network.StopClient();
            connectBtn.gameObject.SetActive(true);
            networkIP.gameObject.SetActive(true);
            hostBtn.gameObject.SetActive(true);
            connectBtn.gameObject.SetActive(true);
            ConnectedText.gameObject.SetActive(false);
            cancelConnectionBtn.gameObject.SetActive(false);
        }

        

    }
}

