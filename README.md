![image](https://github.com/CodeHedge/SysBot.Remote/assets/35341367/ec1b8901-8b20-4348-ad51-3fb41dbac4ba)
![image](https://github.com/CodeHedge/SysBot.Remote/assets/35341367/b9ef1424-03b4-44e9-adac-91d251bd24f5)




# SysBotRemote

SysBotRemote is a robust application that enables users to automate button presses on their Nintendo Switch consoles remotely. With support for multiple switches at once and Discord bot integration, SysBotRemote is a powerful tool for automating in-game actions, testing, and more. The application includes features for recording sequences of button presses (macros), playing them back, as well as a live mode and Discord bot commands for remote control.

## Features

1. **Macro Recording and Playback**: Record sequences of button presses, store them as macros, and play them back at will. Ideal for automating repetitive in-game tasks, testing game functionality, or simulating player interactions.

2. **Multiple Switch Support**: Interact with multiple Nintendo Switch consoles simultaneously. Synchronize actions across multiple games or conduct large-scale tests.

3. **Live Mode**: Transforms into a remote controller for your Nintendo Switch console. Every button press in the application is sent in real-time to all selected switches.

4. **Discord Bot Integration**: Check the network status of the switches, retrieve macro and IP info, and send macros to switches over Discord. Commands include `!status`, `!data`, and `!"macroname" "switchname"`.

## Getting Started

### Prerequisites

- Make sure your Nintendo Switch console(s) are set up for remote access by installing Sysbot.Base. You will need the IP address and port number (usually 6000) for each console.
- For Discord Bot Integration, the bot needs all presence intents and all message permissions when generating the bot.

### Installation

1. Clone the SysBotRemote repository or download the source code.
2. Open the solution file in your preferred C# IDE (e.g., Visual Studio).
3. Compile the solution to produce an executable file.

### Usage

1. Launch the SysBotRemote application.
2. Enter the IP addresses of your Nintendo Switch console(s) in the IP List.
3. Use the button interface to record a sequence of button presses. These will appear as text commands in the macro textbox.
4. Save your macro for future use or press the Play button to execute the macro on the selected consoles.
5. Switch to Live Mode to use SysBotRemote as a remote controller.
6. Use Discord commands for additional remote functionalities.

## Contribution

Contributions to SysBotRemote are welcome! Please fork the repository and create a pull request with your changes.

## License

SysBotRemote is open-source software, licensed under the GNU GENERAL PUBLIC LICENSE Version 3.

## Disclaimer

SysBotRemote is not affiliated with or endorsed by Nintendo Co., Ltd. Use this software responsibly and ensure that all actions comply with local laws and the terms and conditions of Nintendo's software.
