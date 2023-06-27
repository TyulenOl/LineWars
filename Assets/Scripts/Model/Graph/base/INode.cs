using System.Collections.Generic;

namespace Model.Graph
{
    public interface INode
    {
        public IReadOnlyCollection<IEdge> Edges { get; }

        public IEnumerable<INode> GetNeighbors()
        {
            foreach (var edge in Edges)
            {
                if (edge.FirsNode.Equals(this))
                    yield return edge.SecondNode;
                else
                    yield return edge.FirsNode;
            }
        }
    }
}