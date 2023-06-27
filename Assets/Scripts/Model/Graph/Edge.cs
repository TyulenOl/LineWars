using UnityEngine;

namespace Model.Graph
{
    public class Edge: MonoBehaviour, IEdge
    {
        [SerializeField] private Node firstNode;
        [SerializeField] private Node secondNode;
        public INode FirsNode => firstNode;
        public INode SecondNode => secondNode;
    }
}