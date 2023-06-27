using UnityEngine;

namespace Interface
{
    public interface ILineDrawer
    {
        public void Initialise(Transform first, Transform second);
        public void DrawLine();
    }
}