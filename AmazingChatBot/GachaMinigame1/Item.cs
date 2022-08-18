using System.Collections.Generic;

namespace AmazingChatBot.GachaMinigame1
{
    public class Item
    {
        public List<string> Weapons;
        public List<string> Characters;
        public Item()
        {
            Weapons = new List<string>();
            Characters = new List<string>();
        }

        public override string ToString()
        {
            return $"W: {string.Join(',', Weapons)} - C: {string.Join(',', Characters)}";
        }
    }
}
