using Model.Graph;

namespace Model
{
    public interface IUnit: IIdentityObject, IAlive, IHitHandler, IHitCreator, IMovable
    {
        public int Armor { get; }
        public int MeleeDamage { get; }
        public int Speed { get; }
        public UnitSize GetSize();
        public LineType GetMinimaLineType();
    }
}