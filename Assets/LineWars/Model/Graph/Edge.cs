using System.Linq;
using Controllers;
using Extension;
using Interface;
using Mirror;
using UnityEngine;

namespace Model.Graph
{
    public class Edge: NetworkBehaviour, IEdge, IAlive
    {
        private static int index;
        
        [SerializeField] [ReadOnlyInspector] [SyncVar] private int hp;
        
        [SerializeField] [ReadOnlyInspector] private Node firstNode;
        [SerializeField] [ReadOnlyInspector] private Node secondNode;
        [SerializeField] [ReadOnlyInspector] private LineDrawer drawer;

        public int Hp => hp;
        public INode FirsNode => firstNode;
        public INode SecondNode => secondNode;
        public LineDrawer Drawer => drawer;

        public override void OnStartClient()
        {
            Debug.Log("Edge OnStartClient");
            base.OnStartClient();

            var graph = Graph.Instance;
            var allNodes = graph
                .GetNodes()
                .Select(x => (Node) x)
                .ToArray();
            
            transform.SetParent(graph.transform);
                
            var edgeData = LineWarsNetworkManager.Instance.GetGraphData().Edges[index];
                
            var node1 = allNodes[edgeData.Item1];
            var node2 = allNodes[edgeData.Item2];
                
            Initialize(node1, node2);
            node1.AddEdge(this);
            node2.AddEdge(this);
                
            index++;
        }
        
        public void Initialize(Node firstNode, Node secondNode)
        {
            this.firstNode = firstNode;
            this.secondNode = secondNode;
            drawer = GetComponent<LineDrawer>();
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