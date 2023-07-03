using System;
using Mirror;
using UnityEngine;

namespace Controllers
{
    public class LineWarsNetworkManager: NetworkManager
    {
        public override void OnStartServer()
        {
            base.OnStartServer();
            LevelManager.LoadLevel(1);
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            LevelManager.LoadLevel(1);
        }
    }
}