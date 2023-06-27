using Model.Graph;

namespace Model
{
    // Структура необходимая для сохранения текущих характеристик юнита
    public struct BaseUnitCharacteristics
    {
        public int Hp;
        public int Armor;
        public int MeleeDamage;
        public int Speed;
        public UnitSize UnitSize;
        public LineType MovingLineType;

        public BaseUnitCharacteristics(InitialBaseUnitCharacteristics characteristics)
        {
            Hp = characteristics.MaxHp;
            Armor = characteristics.MaxArmor;
            MeleeDamage = characteristics.MeleeDamage;
            Speed = characteristics.Speed;
            UnitSize = characteristics.UnitSize;
            MovingLineType = characteristics.MovingLineType;
        }
    }
}