using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace SysbotMacro.Discord
{
    internal class DiscordBot
    {
        string _token;
        public Action<string> LogAction { get; set; }

        private DiscordSocketClient _client;
        private readonly List<ulong> _sudoUserIds;  // List of user IDs who can run commands
        private readonly List<ulong> _channelIds;  // List of channel IDs where the bot will listen

        public DiscordBot(string token, List<ulong> sudos, List<ulong> channelids)
        {
            _token = token;
            _sudoUserIds = sudos;
            _channelIds = channelids;
            //log token _sudoUsersIds _channelIds
            LogAction?.Invoke(_sudoUserIds.ToString() + " " + _channelIds.ToString());

            var config = new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                // Specify only the intents you need to minimize resource usage
                GatewayIntents = GatewayIntents.GuildMessages | GatewayIntents.MessageContent | GatewayIntents.DirectMessages | GatewayIntents.AllUnprivileged
            };

            _client = new DiscordSocketClient(config);
        }


        public async Task MainAsync()
        {
            _client.Log += LogAsync;
            _client.MessageReceived += MessageReceivedAsync;

            await _client.LoginAsync(TokenType.Bot, _token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage log)
        {
            LogAction?.Invoke(log.ToString());
            return Task.CompletedTask;
        }
        private MessageHandler _messageHandler = new MessageHandler();

        private async Task MessageReceivedAsync(SocketMessage message)
        {
            string logText = $"Received message from {message.Author.Id} in channel {message.Channel.Id}: {message.Content}";
            LogAction?.Invoke(logText);

            LogAction?.Invoke($"Checking if user {message.Author.Id} is in sudo list...");
            LogAction?.Invoke($"Checking if channel {message.Channel.Id} is in channel list...");

            if (!_sudoUserIds.Contains(message.Author.Id))
            {
                LogAction?.Invoke($"User {message.Author.Id} not found in sudo list.");
                return;
            }
            if (!_channelIds.Contains(message.Channel.Id))
            {
                LogAction?.Invoke($"Channel {message.Channel.Id} not found in channel list.");
                return;
            }

            await _messageHandler.HandleMessage(message, async (response) =>
            {
                await message.Channel.SendMessageAsync(response);
            });

        }
        public async Task StopAsync()
        {
            await _client.LogoutAsync();
            await _client.StopAsync();
        }
    }
}
