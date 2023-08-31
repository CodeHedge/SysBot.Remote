using SysBot.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace SysbotMacro
{

    public class Bot : SwitchSocket
    {
        public IWirelessConnectionConfig Config { get; private set; }

        public Bot(IWirelessConnectionConfig config) : base(config)
        {
            this.Config = config;
        }

        public override void Connect()
        {
            Connection.Connect(Info.IP, Info.Port);
        }

        public override void Disconnect()
        {
            Connection.Disconnect(false);
        }

        public override void Reset()
        {
            Disconnect();
            Connect();
        }

        public async Task PressHomeButton()
        {
            var command = SwitchCommand.Click(SwitchButton.HOME);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressAButton()
        {
            var command = SwitchCommand.Click(SwitchButton.A);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressBButton()
        {
            var command = SwitchCommand.Click(SwitchButton.B);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressXButton()
        {
            var command = SwitchCommand.Click(SwitchButton.X);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressYButton()
        {
            var command = SwitchCommand.Click(SwitchButton.Y);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressLButton()
        {
            var command = SwitchCommand.Click(SwitchButton.L);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressRButton()
        {
            var command = SwitchCommand.Click(SwitchButton.R);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressZLButton()
        {
            var command = SwitchCommand.Click(SwitchButton.ZL);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressZRButton()
        {
            var command = SwitchCommand.Click(SwitchButton.ZR);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressPlusButton()
        {
            var command = SwitchCommand.Click(SwitchButton.PLUS);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressMinusButton()
        {
            var command = SwitchCommand.Click(SwitchButton.MINUS);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressLeftStickButton()
        {
            var command = SwitchCommand.Click(SwitchButton.LSTICK);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressRightStickButton()
        {
            var command = SwitchCommand.Click(SwitchButton.RSTICK);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressCaptureButton()
        {
            var command = SwitchCommand.Click(SwitchButton.CAPTURE);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressDPadUp()
        {
            var command = SwitchCommand.Click(SwitchButton.DUP);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressDPadDown()
        {
            var command = SwitchCommand.Click(SwitchButton.DDOWN);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressDPadLeft()
        {
            var command = SwitchCommand.Click(SwitchButton.DLEFT);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public async Task PressDPadRight()
        {
            var command = SwitchCommand.Click(SwitchButton.DRIGHT);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        public byte[] PixelPeek()
        {
            // Original data in hexadecimal format
            byte[] hexData = SwitchCommand.PixelPeek();
            Console.WriteLine($"Received {hexData.Length} bytes.");

            // Convert the byte array to a hexadecimal string
            string hexString = Encoding.UTF8.GetString(hexData);
            Console.WriteLine($"Hex string length: {hexString.Length}");
            Console.WriteLine($"Hex string: {hexString}");

            // Convert the hexadecimal string back to a byte array
            try
            {
                byte[] actualImageBytes = Enumerable.Range(0, hexString.Length)
                                 .Where(x => x % 2 == 0)
                                 .Select(x => Convert.ToByte(hexString.Substring(x, 2), 16))
                                 .ToArray();
                return actualImageBytes;
            }
            catch (FormatException e)
            {
                Console.WriteLine($"FormatException caught: {e.Message}");
                return null;
            }
        }

        private Dictionary<string, SwitchButton> buttonMap = new Dictionary<string, SwitchButton>
    {
        {"A", SwitchButton.A},
        {"B", SwitchButton.B},
        {"X", SwitchButton.X},
        {"Y", SwitchButton.Y},
        {"L", SwitchButton.L},
        {"R", SwitchButton.R},
        {"ZL", SwitchButton.ZL},
        {"ZR", SwitchButton.ZR},
        {"+", SwitchButton.PLUS},
        {"-", SwitchButton.MINUS},
        {"LTS", SwitchButton.LSTICK},
        {"RTS", SwitchButton.RSTICK},
        {"H", SwitchButton.HOME},
        {"SS", SwitchButton.CAPTURE},
        {"UP", SwitchButton.DUP},
        {"DOWN", SwitchButton.DDOWN},
        {"LEFT", SwitchButton.DLEFT},
        {"RIGHT", SwitchButton.DRIGHT},
    };

        public async Task ExecuteCommands(string commands, Func<bool> loopFunc, CancellationToken cancellationToken)
        {
            var splitCommands = commands.TrimEnd().Split(' ');

            int defaultDelay = 100;  // Default delay after each command

            for (int i = 0; i < splitCommands.Length; i++)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                var command = splitCommands[i];

                // If the command is followed by a "d" and a number, interpret it as a delay
                if (i < splitCommands.Length - 2 && splitCommands[i + 1].StartsWith("d") && int.TryParse(splitCommands[i + 1].Substring(1), out var delay))
                {
                    await PressButton(command);
                    await Task.Delay(delay);
                    defaultDelay = delay;  // Update the default delay
                    i++; // Skip the next item in the loop, since we've already processed it
                }
                else
                {
                    await PressButton(command);
                    await Task.Delay(defaultDelay);  // Wait for the default delay
                }
            }

            if (loopFunc() && !cancellationToken.IsCancellationRequested)
            {
                await ExecuteCommands(commands, loopFunc, cancellationToken);
            }
        }


        private async Task PressButton(string button)
        {
            if (!buttonMap.TryGetValue(button.ToUpper(), out var switchButton))
            {
                // The button is not in the dictionary, so we skip it
                return;
            }

            var command = SwitchCommand.Click(switchButton);
            await Connection.SendAsync(new ArraySegment<byte>(command), SocketFlags.None);
        }

        private async Task HoldButton(string button, int duration)
        {
            var switchButton = buttonMap[button.ToUpper()];
            var holdCommand = SwitchCommand.Hold(switchButton);
            var releaseCommand = SwitchCommand.Release(switchButton);
            await Connection.SendAsync(new ArraySegment<byte>(holdCommand), SocketFlags.None);
            await Task.Delay(duration);
            await Connection.SendAsync(new ArraySegment<byte>(releaseCommand), SocketFlags.None);
        }
    }
}



