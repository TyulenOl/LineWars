namespace Model
{
    public struct Hit
    {
        private uint sourceId;
        private uint destinationId;
        private int damage;


        public Hit(uint sourceId, uint destinationId, int damage)
        {
            this.sourceId = sourceId;
            this.destinationId = destinationId;
            this.damage = damage;
        }
        
        public uint SourceId => sourceId;
        public uint DestinationId => destinationId;
        public int Damage => damage;
    }
}