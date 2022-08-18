namespace AmazingChatBot.GachaMinigame1
{
    public class ItemList
    {
        public double DropChance;
        public int Tier;
        public DropChances ValueRanges;
        public Item Items;

        public ItemList()
        {
            Items = new Item();
            ValueRanges = new DropChances();

            DropChance = ValueRanges.Min + ValueRanges.Max;
        }

        public override string ToString()
        {
            return $"T: {Tier} | VR: {ValueRanges} | I: {Items}";
        }
    }
}
