using AmazingChatBot.GachaMinigame1;

namespace AmazingChatBot
{
    public class User
    {
        public string Name;
        public PlayerData GachaMinigame1Data;

        public User(string name)
        {
            Name = name;
            GachaMinigame1Data = new PlayerData() { Name = Name };
        }

        public override string ToString()
        {
            return $"{Name} | {GachaMinigame1Data}";
        }
    }
}
