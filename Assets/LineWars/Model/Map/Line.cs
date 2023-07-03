using System;
using System.Collections.Generic;
using Extension;
using Mirror;
using UnityEngine;

namespace Model.Graph
{
    public class Line: NetworkBehaviour, IAlive, IHitHandler
    {
        [SerializeField] [Min(1)] private int maxHp;
        [SerializeField] [ReadOnlyInspector] [SyncVar] private int hp;
        
        private IEdge edge;

        public int Hp => hp;
        private void Awake()
        {
            edge = GetComponent<IEdge>();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            hp = maxHp;
        }

        
        public void Accept(Hit hit)
        {
            //TODO:Реализовать
        }
    }
}