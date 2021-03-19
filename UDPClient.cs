using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UDP
{
    
    public class UDPClient : MonoBehaviour
    {
        public GameObject UDPServer;
        public Button startServer;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void onStartServer()
        {
            
            if (UDPSocket.server != null)
            {
                UDPSocket.server.Server("127.0.0.1", 27000);
            }
        }
    }
}
