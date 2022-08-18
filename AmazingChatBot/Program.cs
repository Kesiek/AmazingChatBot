using AmazingChatBot.GachaMinigame1;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace AmazingChatBot
{
    class Program
    {
        public static string ChannelName { get; private set; }
        public static string ChannelId { get; private set; }
        public static string BotName { get; private set; }
        public static string BotAuthToken { get; private set; } //https://twitchapps.com/tmi/
        public static string BotAccessToken { get; private set; } //https://twitchtokengenerator.com/ user_read, chat_login, channel:read:redemptions
        public static string GachaMinigame1Pull1Name { get; private set; }
        public static string GachaMinigame1Pull10Name { get; private set; }
        public static bool GachaMinigame1Enabled { get; private set; }
        public static bool UnitConverterEnabled { get; private set; }
        public static bool UnitConverterFeetEnabled { get; private set; }
        public static bool UnitConverterInchesEnabled { get; private set; }
        public static bool UnitConverterMilesEnabled { get; private set; }
        public static bool UnitConverterFahrenheitEnabled { get; private set; }
        public static bool UnitConverterCelsiusEnabled { get; private set; }
        public static bool UnitConverterKilometersEnabled { get; private set; }
        public static bool UnitConverterMetersEnabled { get; private set; }


        public static ConnectionManager ConnectionManager;
        public static GachaMinigame GachaMinigame;
        public static List<User> Users;

        private static readonly string _usersFilePath = $"{Directory.GetCurrentDirectory()}/users.json";
        private static readonly string _configFilePath = $"{Directory.GetCurrentDirectory()}/config.json";

        static async Task Main(string[] args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");

            await LoadConfig();
            await LoadUsersData();

            GachaMinigame = new GachaMinigame();
            await GachaMinigame.Init();

            ConnectionManager = new ConnectionManager();
            ConnectionManager.SetupBot();
            Console.ReadLine();
        }
        public static void DisplayMessageInConsole(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($"{DateTime.Now:hh:mm:ss} - {message}");
            Console.ResetColor();
        }

        private static async Task LoadConfig()
        {
            string json = await SaveLoadManager.LoadFileAsync(_configFilePath);
            JObject obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(json);
            ChannelName = obj.Value<string>("channelName");
            ChannelId = obj.Value<string>("channelId");
            BotName = obj.Value<string>("botAccountName");
            BotAuthToken = obj.Value<string>("botAuthToken");
            BotAccessToken = obj.Value<string>("botAccessToken");
            GachaMinigame1Pull1Name = obj.Value<string>("gachaMinigame1Pull1Name");
            GachaMinigame1Pull10Name = obj.Value<string>("gachaMinigame1Pull10Name");
            GachaMinigame1Enabled = obj.Value<bool>("gachaMinigame1Enabled");
            UnitConverterEnabled = obj.Value<bool>("unitConverterEnabled");
            UnitConverterFeetEnabled = obj.Value<bool>("unitConverterFeetEnabled");
            UnitConverterInchesEnabled = obj.Value<bool>("unitConverterInchesEnabled");
            UnitConverterMilesEnabled = obj.Value<bool>("unitConverterMilesEnabled");
            UnitConverterFahrenheitEnabled = obj.Value<bool>("unitConverterFahrenheitEnabled");
            UnitConverterCelsiusEnabled = obj.Value<bool>("unitConverterCelsiusEnabled");
            UnitConverterKilometersEnabled = obj.Value<bool>("unitConverterKilometersEnabled");
            UnitConverterMetersEnabled = obj.Value<bool>("unitConverterMetersEnabled"); 
        }

        private static async Task LoadUsersData()
        {
            string json = await SaveLoadManager.LoadFileAsync(_usersFilePath);
            if (json == null)
                Users = new List<User>();
            else
                Users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<User>>(json);
        }
    }
}
