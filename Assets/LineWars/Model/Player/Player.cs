using System;
using Controllers;
using LineWars.Model.Game;
using Mirror;
using UnityEngine;

namespace LineWars.Model.Player
{
    public class Player: NetworkBehaviour
    {
        private NationType nationType;
        private INation nation;

        public void Awake()
        {
            nationType = NationType.Russia;
            nation = NationController.GetNation(nationType);
        }

        public GameObject GetUnitPrefab(UnitType type)
        {
            return nation.GetUnitPrefab(type);
        }
    }
}