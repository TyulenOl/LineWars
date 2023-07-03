using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

namespace Model.Graph
{
    public class Graph: MonoBehaviour, IGraph
    {
        public IEnumerable<INode> GetNodes() => FindNodes();

        private IEnumerable<INode> FindNodes()
        {
            return transform.GetChildes()
                .Select(child => child.GetComponent<INode>())
                .Where(node => node != null);
        }
    }
}