using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Model.Graph
{
    public class Point: NetworkBehaviour, IHitHandler, IHitCreator
    {
        private INode node;
        private List<IUnit> units;

        private void Awake()
        {
            node = GetComponent<INode>();
        }

        public void Accept(Hit hit)
        {
            throw new System.NotImplementedException();
        }

        public Hit GenerateHit()
        {
            throw new System.NotImplementedException();
        }
    }
}