using System;
using System.Collections.Generic;
using AmazingChatBot.UnitConverter;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchLib.PubSub;

namespace AmazingChatBot
{
    class ConnectionManager
    {
        private TwitchClient _client;
        private TwitchPubSub _pubSub;

        public void SetupBot()
        {
            Program.DisplayMessageInConsole($"SetupBot: 1", ConsoleColor.Cyan);

            ConnectionCredentials c = new ConnectionCredentials(Program.BotName, Program.BotAuthToken);
            var clientOptions = new ClientOptions()
            {
                MessagesAllowedInPeriod = 20,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };
            WebSocketClient wsc = new WebSocketClient(clientOptions);
            _client = new TwitchClient(wsc);
            _client.Initialize(c, Program.ChannelName);
            _client.OnConnected += Client_OnConnected;
            _client.OnDisconnected += Client_OnDisconnected;
            _client.OnReconnected += Client_OnReconnected;
            _client.OnLog += Client_OnLog;
            _client.OnMessageReceived += Client_OnMessageReceived;
            _client.OnError += Client_OnError;

            Program.DisplayMessageInConsole($"SetupBot: 2", ConsoleColor.Cyan);

            _pubSub = new TwitchPubSub();
            _pubSub.ListenToChannelPoints(Program.ChannelId);
            _pubSub.OnPubSubServiceConnected += PubSub_OnPubSubServiceConnected;
            _pubSub.OnListenResponse += PubSub_OnListenResponse;
            _pubSub.OnChannelPointsRewardRedeemed += PubSub_OnChannelPointsRewardRedeemed;
            _pubSub.OnLog += PubSub_OnLog;

            Program.DisplayMessageInConsole($"SetupBot: 3", ConsoleColor.Cyan);

            _client.Connect();
            _pubSub.Connect();
            Program.DisplayMessageInConsole($"Setup done", ConsoleColor.Cyan);
        }

        private void PubSub_OnChannelPointsRewardRedeemed(object sender, TwitchLib.PubSub.Events.OnChannelPointsRewardRedeemedArgs e)
        {
            if (e.RewardRedeemed.Redemption.Reward.Title == Program.GachaMinigame1Pull1Name && Program.GachaMinigame1Enabled)
            {
                SendMessage(Program.GachaMinigame.GetRandomItem(e.RewardRedeemed.Redemption.User.DisplayName));
                Program.GachaMinigame.SavePlayersData();
            }

            if (e.RewardRedeemed.Redemption.Reward.Title == Program.GachaMinigame1Pull10Name && Program.GachaMinigame1Enabled)
            {
                List<string> rolls = new List<string>();

                for (int i = 0; i < 10; i++)
                {
                    rolls.Add(Program.GachaMinigame.GetRandomItem(e.RewardRedeemed.Redemption.User.DisplayName));
                }
                SendMessage($"Rolled items: {string.Join(", ", rolls)}");
                Program.GachaMinigame.SavePlayersData();
            }
        }
        private void PubSub_OnLog(object sender, TwitchLib.PubSub.Events.OnLogArgs e)
        {
            Program.DisplayMessageInConsole($"PubSub_OnLog: {e.Data}", ConsoleColor.Cyan);
        }
        private void PubSub_OnListenResponse(object sender, TwitchLib.PubSub.Events.OnListenResponseArgs e)
        {
            Program.DisplayMessageInConsole($"PubSub_OnListenResponse: {e.Topic}", ConsoleColor.Green);
        }
        private void PubSub_OnPubSubServiceConnected(object sender, EventArgs e)
        {
            Program.DisplayMessageInConsole("PubSub_OnPubSubServiceConnected", ConsoleColor.Red);
            _pubSub.SendTopics(Program.BotAuthToken);
        }

        private void Client_OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
        {
            Program.DisplayMessageInConsole("Client connected", ConsoleColor.Red);
            Program.DisplayMessageInConsole("Bot should be connected and ready to go", ConsoleColor.Red);
            SendMessage("Bot connected");
        }
        private void Client_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            Program.DisplayMessageInConsole($"Client received message: {e.ChatMessage.Message}", ConsoleColor.Yellow);
            if (e.ChatMessage.Message.ToLowerInvariant().StartsWith("!check ") && Program.GachaMinigame1Enabled)
            {
                var query = e.ChatMessage.Message.Split(" ");
                if (query.Length > 1)
                {
                    var result = Program.GachaMinigame.CheckAmountOfDroppedItem(e.ChatMessage.Username, string.Join(" ", query, 1, query.Length - 1).ToLowerInvariant());
                    SendMessage(result);
                }
            }

            if (e.ChatMessage.Message.ToLowerInvariant() == "!checkdrops" && Program.GachaMinigame1Enabled)
                SendMessage(Program.GachaMinigame.CheckAllDrops(e.ChatMessage.Username));

            if (Program.UnitConverterEnabled)
                SendMessage(Converter.Process(e.ChatMessage.Message.ToLower()));
        }
        private void Client_OnError(object sender, TwitchLib.Communication.Events.OnErrorEventArgs e)
        {
            Program.DisplayMessageInConsole($"Client error occured: {e.Exception.Message}", ConsoleColor.Red);
        }
        private void Client_OnLog(object sender, TwitchLib.Client.Events.OnLogArgs e)
        {
            Program.DisplayMessageInConsole($"Client LOG: {e.Data}", ConsoleColor.White);
        }
        private void Client_OnReconnected(object sender, TwitchLib.Communication.Events.OnReconnectedEventArgs e)
        {
            Program.DisplayMessageInConsole("Client has reconnected", ConsoleColor.Red);
        }
        private void Client_OnDisconnected(object sender, TwitchLib.Communication.Events.OnDisconnectedEventArgs e)
        {
            Program.DisplayMessageInConsole("Client has disconnected", ConsoleColor.Red);
        }

        private void SendMessage(string msg)
        {
            _client.SendMessage(Program.ChannelName, msg);
        }
    }
}
