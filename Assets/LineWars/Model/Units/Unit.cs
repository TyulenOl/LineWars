using System;
using Mirror;
using Model.Graph;
using UnityEngine;
using UnityEngine.Serialization;

namespace Model
{
    public class Unit: NetworkBehaviour, IUnit
    { 
        [SerializeField] private InitialBaseUnitCharacteristics initialBaseUnitCharacteristics;
        
        protected BaseUnitCharacteristics BaseUnitCharacteristics;
        
        protected NetworkIdentity NetworkIdentity;
        protected UnitMovementLogic MovementLogic;

        public uint Id => NetworkIdentity.assetId;
        public int Hp => BaseUnitCharacteristics.Hp;
        public int Armor => BaseUnitCharacteristics.Armor;
        public int MeleeDamage => BaseUnitCharacteristics.MeleeDamage;
        public int Speed => BaseUnitCharacteristics.Speed;
        public UnitSize GetSize() => BaseUnitCharacteristics.UnitSize;
        public LineType GetMinimaLineType() => BaseUnitCharacteristics.MovingLineType;
        
        private void Awake()
        {
            BaseUnitCharacteristics = new BaseUnitCharacteristics(initialBaseUnitCharacteristics);
            NetworkIdentity = GetComponent<NetworkIdentity>();
            MovementLogic = GetComponent<UnitMovementLogic>();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (GetComponent<UnitMovementLogic>() == null)
            {
                Debug.LogWarning($"у {name} не обнаружен компонент {nameof(UnitMovementLogic)}");
            }
        }

        public void Accept(Hit hit)
        {
            throw new NotImplementedException();
        }

        public Hit GenerateHit()
        {
            throw new NotImplementedException();
        }

        public void MoveTo(INode targetNode)
        {
            MovementLogic.MoveTo(targetNode.Position);
        }
    }
}