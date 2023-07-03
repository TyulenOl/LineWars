using System;
using Mirror;

namespace Controllers
{
    public class LineWarsNetworkManager: NetworkManager
    {
        public override void OnStartServer()
        {
            base.OnStartServer();
            LevelManager.LoadLevel(1);
        }
    }
}