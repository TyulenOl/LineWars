using Extension;
using Interface;
using UnityEngine;

namespace Model.Graph
{
    public class Edge: MonoBehaviour, IEdge
    {
        [SerializeField] [ReadOnlyInspector] private Node firstNode;
        [SerializeField] [ReadOnlyInspector] private Node secondNode;
        [SerializeField] [ReadOnlyInspector] private ILineDrawer drawer;
        public INode FirsNode => firstNode;
        public INode SecondNode => secondNode;

        public void Initialise(Node firstNode, Node secondNode)
        {
            this.firstNode = firstNode;
            this.secondNode = secondNode;
            drawer = GetComponent<ILineDrawer>();
            drawer.Initialise(firstNode.transform, secondNode.transform);
        }

        public void ReDraw()
        {
            drawer.DrawLine();
        }

        public Node GetFirstNode() => firstNode;
        public Node GetSecondNode() => secondNode;
    }
}