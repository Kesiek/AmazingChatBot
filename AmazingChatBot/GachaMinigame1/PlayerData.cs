using System.Collections.Generic;

namespace AmazingChatBot.GachaMinigame1
{
    public class PlayerData
    {
        public string Name;
        public int PityTier4Counter;
        public int PityTier5Counter;
        public string PityTier4LastDrop;
        public string PityTier5LastDrop;
        public List<SavedDrops> Drops;

        public PlayerData()
        {
            Drops = new List<SavedDrops>();
        }

        public override string ToString()
        {
            return $"PC4: {PityTier4Counter} - LD4: {PityTier4LastDrop} | PC5: {PityTier5Counter} - LD5: {PityTier5LastDrop}";
        }

    }
}
