using System;
using System.Collections.Generic;
using Extension;
using Mirror;
using UnityEngine;

namespace Model.Graph
{
    public class Line: MonoBehaviour, IAlive, IHitHandler
    {
        [SerializeField][Min(1)] private int maxHp;
        [SerializeField] [ReadOnlyInspector] private int hp;
        private IEdge edge;

        public int Hp => hp;
        private void Awake()
        {
            edge = GetComponent<IEdge>();
        }

        private void OnValidate()
        {
            hp = maxHp;
        }

        
        public void Accept(Hit hit)
        {
            //TODO:Реализовать
        }
    }
}