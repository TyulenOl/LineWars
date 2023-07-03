using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

namespace Model.Graph
{
    public class Graph: MonoBehaviour, IGraph
    {
        public static Graph Instance { get; private set; }
        private List<INode> nodes;

        public IEnumerable<INode> GetNodes() => nodes ?? FindNodes();
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            nodes = new List<INode>();
        }

        public void RegisterNode(INode node) => nodes.Add(node);

        // onlyEditor
        private IEnumerable<INode> FindNodes()
        {
            return transform.GetChildes()
                .Select(child => child.GetComponent<INode>())
                .Where(node => node != null);
        }
    }
}