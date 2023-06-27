namespace Model.Graph
{
    public interface IEdge
    {
        public INode FirsNode { get; }
        public INode SecondNode { get; }
    }
}