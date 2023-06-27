using System.Collections.Generic;
using UnityEngine;

namespace Model.Graph
{
    public class Node : MonoBehaviour, INode
    {
        [SerializeField] private List<Edge> edges;
        public IReadOnlyCollection<IEdge> Edges => edges;
    }
}