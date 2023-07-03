using System.Collections.Generic;

namespace Model.Graph
{
    public interface IGraph
    {
        public IEnumerable<INode> GetNodes();
    }
}