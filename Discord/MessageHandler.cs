using Discord.WebSocket;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using Discord;
using System.Threading;
using SysbotMacro;
using SysBot.Base;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.Text;


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
                            macroField += $"{kvp.Key}: **{kvp.Value}**, ";
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
                            ipField += $"{kvp.Key}: **{kvp.Value}**, ";
                        }
                        ipField = ipField.TrimEnd(',', ' ') + "\n";
                    }
                    embed.AddField("Switch IPs", ipField);

                    await ((ISocketMessageChannel)message.Channel).SendMessageAsync(embed: embed.Build());
                }
                else if (_macroDict.Any(dict => dict.ContainsKey("Name") && dict["Name"].ToString().Equals(command, StringComparison.OrdinalIgnoreCase)))
                {
                    await sendResponse($"Debug: Received command to macro");

                    // Your debug lines for the Macro Dictionary and IP Dictionary can go here.

                    // Assuming your second part is the Switch IP key, e.g., "Hedge"
                    var switchKey = parts.Length > 1 ? parts[1] : string.Empty;
                    if (string.IsNullOrEmpty(switchKey))
                    {
                        await sendResponse("Missing Switch Key for Macro");
                        return;
                    }

                    // Debug: Print what the bot is searching for in terms of Macro
                    await sendResponse($"Debug: Looking for Macro with name {command}");

                    var macroDictEntry = _macroDict.First(dict => dict.ContainsKey("Name") && dict["Name"].ToString().Equals(command, StringComparison.OrdinalIgnoreCase));
                    var macro = macroDictEntry["Macro"].ToString();

                    // Debug: Print what the bot is searching for in terms of IP
                    await sendResponse($"Debug: Looking for Switch IP with key {switchKey}");

                    var switchIpDict = _ipDict.FirstOrDefault(dict => dict.ContainsKey("SwitchName") && dict["SwitchName"].ToString().Equals(switchKey, StringComparison.OrdinalIgnoreCase));
                    var switchIp = switchIpDict != null ? switchIpDict["IPAddress"].ToString() : null;

                    // Debug: Print the found or not-found IP
                    if (switchIp == null)
                    {
                        await sendResponse($"No Switch IP found for key {switchKey}");
                        return;
                    }


                    // Executing the macro
                    await sendResponse($"Executing macro {command} on Switch {switchKey}");
                    var config = new SwitchConnectionConfig
                    {
                        IP = switchIp,
                        Port = 6000,
                        Protocol = SwitchProtocol.WiFi
                    };

                    var bot = new Bot(config);
                    bot.Connect();
                    var cancellationToken = new CancellationToken();  // You can use a real cancellation token
                    await bot.ExecuteCommands(macro, () => false, cancellationToken);  // Replace the loop condition as needed
                    bot.Disconnect();

                    await sendResponse($"Executed macro {command} on Switch {switchKey}");
                }
                else if (command == "status")
                {
                    // Declare a StringBuilder for embed description.
                    StringBuilder embedDescription = new StringBuilder();

                    // Loop through all the IP dictionaries to ping them.
                    foreach (var switchDict in _ipDict)
                    {
                        // Extract the name and the IP.
                        var switchName = switchDict.ContainsKey("SwitchName") ? switchDict["SwitchName"].ToString() : "Unknown";
                        var ipAddress = switchDict.ContainsKey("IPAddress") ? switchDict["IPAddress"].ToString() : "Unknown";

                        // Initialize a new instance of the Ping class.
                        Ping pingSender = new Ping();

                        // Ping the IP address.
                        PingReply reply = await pingSender.SendPingAsync(ipAddress, 2000);  // 2000 ms timeout

                        // Check the status.
                        string status = reply.Status == IPStatus.Success ? "Up" : "Down";

                        // Append to the embed description.
                        embedDescription.AppendLine($"Name: {switchName}, IP: {ipAddress}, Status: {status}");
                    }

                    // Create embed message with Discord.Net's EmbedBuilder.
                    var embed = new EmbedBuilder
                    {
                        Title = "Switch Status Check",
                        Description = embedDescription.ToString(),
                        Color = Color.Green  // You can change the color based on your preferences.
                    };

                    // Send the embed.
                    await ((ISocketMessageChannel)message.Channel).SendMessageAsync(embed: embed.Build());
                }

            }
        }
    }
}
