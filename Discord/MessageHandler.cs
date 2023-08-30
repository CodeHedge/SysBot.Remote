using Discord.WebSocket;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace SysbotMacro.Discord
{
    internal class MessageHandler
    {
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
                // Add more commands here
            }
        }
    }
}
