using System;
using LineWars.Model.Game;
using LineWars.Model.Player;
using Mirror;
using UnityEngine;

namespace Controllers
{
    public class PlayerUnitsController: NetworkBehaviour
    {
        private Player player;

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        public void CreateUnit()
        {
            if(!isLocalPlayer) return;
            var unit = player.GetUnitPrefab(UnitType.Infrantry);
            var instance = Instantiate(unit);
            NetworkServer.Spawn(instance);
        }
    }
}