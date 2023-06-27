using System;
using Mirror;
using Model.Graph;
using UnityEngine;
using UnityEngine.Serialization;

namespace Model
{
    public class Unit: MonoBehaviour, IUnit
    { 
        [SerializeField] private InitialBaseUnitCharacteristics initialBaseUnitCharacteristics;
        protected BaseUnitCharacteristics baseUnitCharacteristics;

        private NetworkIdentity networkIdentity;

        private void Awake()
        {
            baseUnitCharacteristics = new BaseUnitCharacteristics(initialBaseUnitCharacteristics);
            networkIdentity = GetComponent<NetworkIdentity>();
        }

        public uint Id => networkIdentity.assetId;
        public int Hp => baseUnitCharacteristics.Hp;
        public int Armor => baseUnitCharacteristics.Armor;
        public int MeleeDamage => baseUnitCharacteristics.MeleeDamage;
        public int Speed => baseUnitCharacteristics.Speed;
        public UnitSize GetSize() => baseUnitCharacteristics.UnitSize;
        public LineType GetMinimaLineType() => baseUnitCharacteristics.MovingLineType;
        
        public void Accept(Hit hit)
        {
            throw new NotImplementedException();
        }

        public Hit GenerateHit()
        {
            throw new NotImplementedException();
        }

      
    }
}