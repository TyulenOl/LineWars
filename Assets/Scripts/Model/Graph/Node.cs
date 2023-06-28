using System;
using System.Collections.Generic;
using System.Linq;
using Extension;
using Unity.Collections;
using UnityEngine;

namespace Model.Graph
{
    public class Node : MonoBehaviour, INode
    {
        [SerializeField] [ReadOnlyInspector] private List<Edge> edges;
        public IReadOnlyCollection<IEdge> Edges => edges;
        public List<Edge> GetEdgesList() => edges;
        public void Initialise()
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

    }
}