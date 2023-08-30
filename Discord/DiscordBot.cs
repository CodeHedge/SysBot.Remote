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
        private readonly List<ulong> _sudoUserIds;
        private readonly List<ulong> _channelIds;
        private readonly List<Dictionary<string, object>> _ipDict;
        private readonly List<Dictionary<string, object>> _macroDict;

        private MessageHandler _messageHandler;

        public DiscordBot(string token, List<ulong> sudos, List<ulong> channelids, List<Dictionary<string, object>> ipDict, List<Dictionary<string, object>> macroDict)
        {
            _token = token;
            _sudoUserIds = sudos;
            _channelIds = channelids;
            _ipDict = ipDict;
            _macroDict = macroDict;
            _messageHandler = new MessageHandler(_ipDict, _macroDict);

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
