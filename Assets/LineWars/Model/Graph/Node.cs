using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using Extension;
using Interface;
using Mirror;
using UnityEngine;

namespace Model.Graph
{
    public class Node : NetworkBehaviour, INode
    {
        [SyncVar] private uint id;
        
        [SerializeField] [ReadOnlyInspector] private List<Edge> edges;
        [SerializeField] private Outline2D outline;
        
        public Vector2 Position => transform.position;
        public IReadOnlyCollection<IEdge> Edges => edges;
        public IReadOnlyList<Edge> GetEdgesList() => edges;

        public uint Id
        {
            get => id;
            set => id = value;
        }

        public override void OnStartClient()
        {
            Debug.Log("Node OnStartClient");
            base.OnStartClient();
            
            Initialize();
            Graph.Instance.RegisterNode(this);
            transform.SetParent(Graph.Instance.transform);
            transform.position = 
                LineWarsNetworkManager.Instance
                .GetGraphData()
                .NodesPositions[id];
        }

        public void Initialize()
        {
            edges = new List<Edge>();
        }

        public void BeforeDestroy(out List<Edge> deletedEdges, out List<Node> neighbors)
        {
            neighbors = new List<Node>();
            deletedEdges = edges.ToList();
            
            foreach (var edge in edges)
            {
                var first = edge.GetFirstNode();
                var second = edge.GetSecondNode();
                if (first.Equals(this))
                {
                    second.RemoveEdge(edge);
                    neighbors.Add(second);
                }
                else
                {
                    first.RemoveEdge(edge);
                    neighbors.Add(first);
                }
            }

            edges = null;
        }
        
        public void AddEdge(Edge edge)
        {
            if (edges.Contains(edge))
                return;
            edges.Add(edge);
        }

        public bool RemoveEdge(Edge edge) => edges.Remove(edge);

        public void SetActiveOutline(bool value) => outline.SetActiveOutline(value);
    }
}