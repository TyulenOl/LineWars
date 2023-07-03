using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

namespace Model.Graph
{
    public class Graph: MonoBehaviour, IGraph
    {
        public static Graph Instance { get; private set; }
        
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
        
        public IEnumerable<INode> GetNodes() => FindNodes();

        private IEnumerable<INode> FindNodes()
        {
            return transform.GetChildes()
                .Select(child => child.GetComponent<INode>())
                .Where(node => node != null);
        }
    }
}