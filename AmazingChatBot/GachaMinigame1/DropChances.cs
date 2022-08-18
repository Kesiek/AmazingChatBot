using System;

namespace AmazingChatBot.GachaMinigame1
{
    public class DropChances
    {
        public double Min;
        public double Max;

        public DropChances()
        {
            RoundValues();
        }

        public void RoundValues()
        {
            Min = Math.Round(Min, 1);
            Max = Math.Round(Max, 1);
        }

        public override string ToString()
        {
            return $"MIN: {Min} - MAX: {Max} ({Max - Min})";
        }
    }
}
