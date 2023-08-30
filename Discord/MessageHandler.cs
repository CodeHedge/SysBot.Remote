using Discord.WebSocket;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using Discord;

namespace SysbotMacro.Discord
{
    internal class MessageHandler
    {
        private readonly List<Dictionary<string, object>> _ipDict;
        private readonly List<Dictionary<string, object>> _macroDict;

        public MessageHandler(List<Dictionary<string, object>> ipDict, List<Dictionary<string, object>> macroDict)
        {
            _ipDict = ipDict;
            _macroDict = macroDict;
        }
        public async Task HandleMessage(SocketMessage message, Func<string, Task> sendResponse)
        {
            // Split the message into words
            var parts = message.Content.Split(' ');
            var firstPart = parts.FirstOrDefault();
            

            // Check if the first word starts with '!'
            if (firstPart != null && firstPart.StartsWith("!"))
            {
                var command = firstPart.Substring(1); // Remove '!'

                // Determine which command it is and execute the corresponding code
                if (command == "ping")
                {
                    await sendResponse("Pong!");
                }
                // command to message in the channel with all macros and switches
                else if (command == "data")
                {
                    if (_macroDict == null || !_macroDict.Any() || _ipDict == null || !_ipDict.Any())
                    {
                        await sendResponse("One or both of the dictionaries are either null or empty.");
                        return;
                    }

                    var embed = new EmbedBuilder
                    {
                        Title = "Macro and Switch IP Data",
                        Color = Color.Blue
                    };

                    string macroField = "";
                    foreach (var dict in _macroDict)
                    {
                        foreach (var kvp in dict)
                        {
                            macroField += $"**{kvp.Key}**: {kvp.Value}, ";
                        }
                        macroField = macroField.TrimEnd(',', ' ') + "\n";
                    }
                    embed.AddField("Macros", macroField);

                    string ipField = "";
                    foreach (var dict in _ipDict)
                    {
                        foreach (var kvp in dict)
                        {
                            if (kvp.Key == "IsChecked") continue;
                            ipField += $"**{kvp.Key}**: {kvp.Value}, ";
                        }
                        ipField = ipField.TrimEnd(',', ' ') + "\n";
                    }
                    embed.AddField("Switch IPs", ipField);

                    await ((ISocketMessageChannel)message.Channel).SendMessageAsync(embed: embed.Build());
                }


            }
        
        }
    }
}
