using UnityEngine;

namespace LineWars.Model.Game.Nations
{
    public class DefaultNation : INation
    {
        public GameObject GetUnitPrefab(UnitType type)
        {
            switch (type)
            {
                case UnitType.Infrantry:
                    return Resources.Load<GameObject>("DefaultUnits/BaseInfrantry");
                default:
                    return Resources.Load<GameObject>("DefaultUnits/BaseInfrantry");;
            }
        }
    }
}