using Model.Graph;

namespace Model
{
    public interface IMovable
    {
        public void MoveTo(INode targetNode);
    }
}