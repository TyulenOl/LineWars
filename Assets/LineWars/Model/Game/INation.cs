using UnityEngine;

namespace LineWars.Model.Game
{
    public interface INation
    {
        public GameObject GetUnitPrefab(UnitType type);
    }
}