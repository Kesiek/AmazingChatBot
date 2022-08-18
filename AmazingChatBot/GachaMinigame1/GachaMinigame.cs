using AmazingChatBot;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingChatBot.GachaMinigame1
{
    class GachaMinigame
    {
        public List<PlayerData> PlayersData;
        public List<ItemList> ItemLists;

        private bool _isCurrentlySaving = false;

        private readonly Random _random;
        private readonly string ConfigFilePath = $"{Directory.GetCurrentDirectory()}/gachaMinigame1Config.json";
        private readonly string SaveFilePath = $"{Directory.GetCurrentDirectory()}/gachaMinigame1SavesDrops.json";

        public GachaMinigame()
        {
            PlayersData = new List<PlayerData>();
            ItemLists = new List<ItemList>();
            _random = new Random();

            if (File.Exists(ConfigFilePath))
            {
                using StreamReader sr = new StreamReader(ConfigFilePath);
                string json = sr.ReadToEnd();
                sr.Close();
                var items = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(json);
                ItemLists = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ItemList>>(items["itemList"].ToString());
            }
        }

        /// <summary>
        /// Simple method that loads player data in to memory and starts autosave to happen every minute
        /// </summary>
        public async Task Init()
        {
            await LoadPlayersData();
            StartAutosave();
        }
        public async Task LoadPlayersData()
        {
            string data = await SaveLoadManager.LoadFileAsync(SaveFilePath);
            if (data == null)
                PlayersData = new List<PlayerData>();
            else
                PlayersData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PlayerData>>(data);
        }

        public void StartAutosave()
        {
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(TimeSpan.FromMinutes(1));
                    SavePlayersData();
                    Program.DisplayMessageInConsole("Autosaved gachaminigame1", ConsoleColor.Magenta);
                }
            });
        }

        public void SavePlayersData()
        {
            _ = Task.Run(async () =>
            {
                if (_isCurrentlySaving)
                    return;
                _isCurrentlySaving = true;

                lock (PlayersData)
                {
                    foreach (var item in PlayersData)
                    {
                        item.Name = item.Name.ToLowerInvariant();
                    }
                }

                var tmpCopy = PlayersData.AsReadOnly();
                await SaveLoadManager.SaveFileAsync(SaveFilePath, tmpCopy);
                _isCurrentlySaving = false;
            });
        }

        public string GetRandomItem(string userName)
        {
            var player = PlayersData.FirstOrDefault(x => x.Name.ToLower() == userName.ToLower());
            if (player == null)
            {
                player = new PlayerData() { Name = userName };
                PlayersData.Add(player);
            }

            string rolledItem = "";
            bool changedValueRanges = false;

            if (player.PityTier4Counter >= 10)
                rolledItem = RollTier4Item(player);
            if (player.PityTier5Counter >= 76 && player.PityTier5Counter < 89)
            {
                SetupValueRanges();
                changedValueRanges = true;
            }
            else if (player.PityTier5Counter >= 90)
                rolledItem = RollTier5Item(player);

            if (rolledItem == "")
            {
                double result = Math.Round(_random.NextDouble() * 100f, 1);
                foreach (var itemsList in ItemLists)
                {
                    if (result >= itemsList.ValueRanges.Min && result <= itemsList.ValueRanges.Max)
                    {
                        if (itemsList.Tier == 4)
                            rolledItem = RollTier4Item(player);
                        else if (itemsList.Tier == 5)
                            rolledItem = RollTier5Item(player);
                        else
                            rolledItem = RollItem(player, itemsList);
                    }
                }
            }

            if (changedValueRanges)
            {
                RestoreValueRanges();
                changedValueRanges = false;
            }

            player.PityTier4Counter++;
            player.PityTier5Counter++;
            return rolledItem;
        }

        public string CheckAmountOfDroppedItem(string userName, string itemName)
        {
            var player = PlayersData.FirstOrDefault(x => x.Name == userName);
            if (player == null)
                return $"{userName} you do not have any drops yet";

            var result = player.Drops.
                Where(x => x.Name.ToLowerInvariant().StartsWith(itemName)).
                GroupBy(x => x.Name).
                Select(x => new { name = x.Key, amount = x.Count() });

            if (result.Count() > 1)
            {
                string names = "";
                foreach (var item in result)
                {
                    names += $"{item.name}, ";
                }
                names = names[0..^2];
                return $"{userName} did you mean: {names}";
            }
            else
            {
                int droppedAmount = player.Drops.Where(x => x.Name.ToLowerInvariant().StartsWith(itemName)).Count();
                if (droppedAmount == 0)
                    return $"{userName} you do not have any drops yet";

                string fullItemName = player.Drops.Where(x => x.Name.ToLowerInvariant().StartsWith(itemName)).Select(x => x.Name).FirstOrDefault();
                return $"{userName} you own: {droppedAmount}x {fullItemName}";
            }
        }

        public string CheckAllDrops(string userName)
        {
            var player = PlayersData.FirstOrDefault(x => x.Name == userName);
            if (player == null)
                return $"{userName} you do not have any drops yet";
            var result = player.Drops.GroupBy(x => x.Name).Select(x => new { name = x.Key, amount = x.Count() });
            if (result.Count() == 0)
                return $"{userName} you do not have any drops yet";

            string msg = $"{userName} you have: ";
            foreach (var item in result)
            {
                msg += $"{item.amount}x {item.name}, ";
            }

            return msg[0..^2];
        }

        private string RollItem(PlayerData player, ItemList item)
        {
            int index = _random.Next(0, item.Items.Weapons.Count);
            if (item.Tier == 6) { SaveDroppedItem(player, item.Items.Weapons[index]); }
            return item.Items.Weapons[index];
        }
        private string RollTier4Item(PlayerData player)
        {
            player.PityTier4Counter = 0;
            if (player.PityTier4LastDrop == "weapon")
            {
                int charIndex = _random.Next(0, ItemLists[2].Items.Characters.Count);
                player.PityTier4LastDrop = "character";
                SaveDroppedItem(player, ItemLists[2].Items.Characters[charIndex]);
                return ItemLists[2].Items.Characters[charIndex];
            }
            else
            {
                int type = _random.Next(0, 2);
                if (type == 0)
                { //weapon
                    int index = _random.Next(0, ItemLists[2].Items.Weapons.Count);
                    player.PityTier4LastDrop = "weapon";
                    SaveDroppedItem(player, ItemLists[2].Items.Weapons[index]);
                    return ItemLists[2].Items.Weapons[index];
                }
                else
                { //character
                    int index = _random.Next(0, ItemLists[2].Items.Characters.Count);
                    player.PityTier4LastDrop = "character";
                    SaveDroppedItem(player, ItemLists[2].Items.Characters[index]);
                    return ItemLists[2].Items.Characters[index];
                }
            }
        }
        private string RollTier5Item(PlayerData player)
        {
            player.PityTier5Counter = 0;
            if (player.PityTier5LastDrop == "weapon")
            {
                int charIndex = _random.Next(0, ItemLists[1].Items.Characters.Count);
                player.PityTier5LastDrop = "character";
                SaveDroppedItem(player, ItemLists[1].Items.Characters[charIndex]);
                return ItemLists[1].Items.Characters[charIndex];
            }
            else
            {
                int type = _random.Next(0, 2);
                if (type == 0)
                { //weapon
                    int index = _random.Next(0, ItemLists[1].Items.Weapons.Count);
                    player.PityTier5LastDrop = "weapon";
                    SaveDroppedItem(player, ItemLists[1].Items.Weapons[index]);
                    return ItemLists[1].Items.Weapons[index];
                }
                else
                { //character
                    int index = _random.Next(0, ItemLists[1].Items.Characters.Count);
                    player.PityTier5LastDrop = "character";
                    SaveDroppedItem(player, ItemLists[1].Items.Characters[index]);
                    return ItemLists[1].Items.Characters[index];
                }
            }
        }

        private static void SaveDroppedItem(PlayerData player, string itemName)
        {
            player.Drops.Add(new SavedDrops(itemName));
        }

        private void SetupValueRanges()
        {
            ItemLists[1].ValueRanges.Max = 32.1f;
            ItemLists[2].ValueRanges.Min = 32.2f;
            ItemLists[2].ValueRanges.Max = 37.3f;
            ItemLists[3].ValueRanges.Min = 37.4f;
            ItemLists[0].ValueRanges.RoundValues();
            ItemLists[1].ValueRanges.RoundValues();
            ItemLists[2].ValueRanges.RoundValues();
            ItemLists[3].ValueRanges.RoundValues();
        }
        private void RestoreValueRanges()
        {
            ItemLists[1].ValueRanges.Max = 0.6f;
            ItemLists[2].ValueRanges.Min = 0.7f;
            ItemLists[2].ValueRanges.Max = 5.8f;
            ItemLists[3].ValueRanges.Min = 5.9f;
            ItemLists[0].ValueRanges.RoundValues();
            ItemLists[1].ValueRanges.RoundValues();
            ItemLists[2].ValueRanges.RoundValues();
            ItemLists[3].ValueRanges.RoundValues();
        }
    }
}
