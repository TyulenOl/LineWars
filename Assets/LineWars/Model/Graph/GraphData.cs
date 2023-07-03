using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model.Graph
{
    public class GraphData: ScriptableObject
    {
        [Serializable]
        public class Edge
        {
            [SerializeField] public int First;
            [SerializeField] public int Second;

            public Edge(int first, int second)
            {
                First = first;
                Second = second;
            }
        }
        
        [SerializeField] [HideInInspector] private Vector2[] nodes;
        [SerializeField] [HideInInspector] private Edge[] edges;
        
        private int currentNodeIndex;
        private int currentEdgeIndex;
        private List<INode> graph;

        public Vector2[] NodesPositions => nodes.ToArray();

        public (int,int)[] Edges => edges
            .Select(x => (x.First,x.Second))
            .ToArray();

        public void Initialize(IReadOnlyCollection<INode> graph)
        {
            var nodesCount = graph.Count;
            nodes = new Vector2[nodesCount];

            var edgesCount = graph
                .SelectMany(x => x.Edges)
                .Distinct()
                .Count();

            edges = new Edge[edgesCount];

            //если граф незвязный, если связный, то итерируется один раз
            this.graph = graph.ToList();
            while (this.graph.Count > 0)
            {
                WidthTraversal(this.graph.First());
            }
        }

        private void WidthTraversal(INode prop)
        {
            var queue = new Queue<INode>();
            var nodeRegister = new Dictionary<INode, int>();
            var openedNodes = new HashSet<INode>();

            queue.Enqueue(prop);
            openedNodes.Add(prop);

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                graph.Remove(currentNode);

                nodes[currentNodeIndex] = currentNode.Position;
                nodeRegister.Add(currentNode, currentNodeIndex);

                foreach (var edge in currentNode.Edges)
                {
                    var otherNode = edge.GetOther(currentNode);
                    if (!nodeRegister.ContainsKey(otherNode))
                    {
                        if (!openedNodes.Contains(otherNode))
                        {
                            queue.Enqueue(otherNode);
                            openedNodes.Add(otherNode);
                        }
                    }
                    else
                    {
                        var otherNodeIndex = nodeRegister[otherNode];
                        edges[currentEdgeIndex] = new Edge(otherNodeIndex, currentNodeIndex);
                        currentEdgeIndex++;
                    }
                }

                currentNodeIndex++;
            }
        }
    }
}