namespace AmazingChatBot.GachaMinigame1
{
    public class SavedDrops
    {
        public string Name;

        public SavedDrops(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"N: {Name}";
        }
    }
}
