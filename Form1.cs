using Newtonsoft.Json;
using SysBot.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using SysbotMacro.Discord;







namespace SysbotMacro
{
    public partial class Form1 : Form
    {
        private bool canSaveData = false;

        // Declare the bots list that will be looped through when commands are sent.
        private List<Bot> bots = new List<Bot>(); 

        // Used for Live button pressing
        private bool live;

        private List<Dictionary<string, object>> ipDict = new List<Dictionary<string, object>>();
        private List<Dictionary<string, object>> macroDict = new List<Dictionary<string, object>>();




        private DiscordBot _bot;




        public Form1()
        {
            InitializeComponent();
            LoadData();
            string _token = discordTokenTB.Text;
        }

        // Store the IP's and the Macros in each list when changes are made. Keep data after the program closes
        private void SaveData()
        {
            if (!canSaveData)
            {
                return; // Skip save if data isn't ready to be saved
            }


            ipDict.Clear(); // Clear before adding new items
            foreach (ListViewItem item in ipListView.Items)
            {
                var itemData = new Dictionary<string, object>
        {
            { "IsChecked", item.Checked },
            { "SwitchName", item.SubItems[1].Text },
            { "IPAddress", item.SubItems[2].Text }
        };
                ipDict.Add(itemData);
            }

            File.WriteAllText("ipListView.json", JsonConvert.SerializeObject(ipDict));


            // Save macroListView

            macroDict.Clear(); // Clear before adding new items
            foreach (ListViewItem item in macroListView.Items)
            {
                var itemData = new Dictionary<string, object>
        {
            { "Name", item.Text },
            { "Macro", item.SubItems[1].Text }
        };
                macroDict.Add(itemData);
            }

            File.WriteAllText("macroListView.json", JsonConvert.SerializeObject(macroDict));
        

        //save discordTokenTB.text to a file
        File.WriteAllText("discordTokenTB.json", JsonConvert.SerializeObject(discordTokenTB.Text));

            // Save userIDLV to a JSON file
            var userIDListViewData = new List<Dictionary<string, object>>();
            foreach (ListViewItem item in userIDLV.Items)
            {
                UpdateLogger($"UserName: {item.Text}, UserID: {item.SubItems[1]?.Text ?? "null"}");
                var itemData = new Dictionary<string, object>
        {
            { "UserName", item.Text },
            { "UserID", item.SubItems[1].Text }
        };
                userIDListViewData.Add(itemData);
            }
            File.WriteAllText("userIDLV.json", JsonConvert.SerializeObject(userIDListViewData));

            // Save channelIDLV to a JSON file
            var channelIDListViewData = new List<Dictionary<string, object>>();
            foreach (ListViewItem item in channelIDLV.Items)
            {
                var itemData = new Dictionary<string, object>
        {
            { "ChannelName", item.Text },
            { "ChannelID", item.SubItems[1].Text }
        };
                channelIDListViewData.Add(itemData);
            }
            File.WriteAllText("channelIDLV.json", JsonConvert.SerializeObject(channelIDListViewData));

            // Save discordTokenTB.text to a file
            File.WriteAllText("discordTokenTB.json", JsonConvert.SerializeObject(discordTokenTB.Text));



        }

        // Loads the stored data back into the lists
        private void LoadData()
        {

            // Load saveCheckList
            if (File.Exists("macroListView.json"))
            {
                var macroListViewDataJson = File.ReadAllText("macroListView.json");
                var macroListViewData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(macroListViewDataJson);

                macroListView.Items.Clear();
                foreach (var itemData in macroListViewData)
                {
                    var newItem = new ListViewItem
                    {
                        Text = itemData["Name"].ToString()
                    };
                    newItem.SubItems.Add(itemData["Macro"].ToString());
                    macroListView.Items.Add(newItem);
                }
            }

            // Load ipListView
            if (File.Exists("ipListView.json"))
            {
                var ipListViewDataJson = File.ReadAllText("ipListView.json");
                var ipListViewData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(ipListViewDataJson);

                ipListView.Items.Clear();
                foreach (var itemData in ipListViewData)
                {
                    var newItem = new ListViewItem
                    {
                        Checked = (bool)itemData["IsChecked"]
                    };
                    newItem.SubItems.Add(itemData["SwitchName"].ToString());
                    newItem.SubItems.Add(itemData["IPAddress"].ToString());
                    ipListView.Items.Add(newItem);
                }
            }
            //load discordTokenTB.text from a file
            if (File.Exists("discordTokenTB.json"))
            {
                var discordTokenTBDataJson = File.ReadAllText("discordTokenTB.json");
                var discordTokenTBData = JsonConvert.DeserializeObject<string>(discordTokenTBDataJson);

                discordTokenTB.Text = discordTokenTBData;
            }

            // Load userIDLV from a JSON file
            if (File.Exists("userIDLV.json"))
            {
                var userIDLVDataJson = File.ReadAllText("userIDLV.json");
                var userIDLVData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(userIDLVDataJson);

                userIDLV.Items.Clear();  // Clear existing items
                foreach (var itemData in userIDLVData)
                {
                    var newItem = new ListViewItem
                    {
                        Text = itemData["UserName"].ToString()
                    };
                    newItem.SubItems.Add(itemData["UserID"].ToString());
                    userIDLV.Items.Add(newItem);
                }
            }

            // Load channelIDLV from a JSON file
            if (File.Exists("channelIDLV.json"))
            {
                var channelIDLVDataJson = File.ReadAllText("channelIDLV.json");
                var channelIDLVData = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(channelIDLVDataJson);

                channelIDLV.Items.Clear();  // Clear existing items
                foreach (var itemData in channelIDLVData)
                {
                    var newItem = new ListViewItem
                    {
                        Text = itemData["ChannelName"].ToString()
                    };
                    newItem.SubItems.Add(itemData["ChannelID"].ToString());
                    channelIDLV.Items.Add(newItem);
                }
            }

            // Load discordTokenTB.text from a file
            if (File.Exists("discordTokenTB.json"))
            {
                var discordTokenTBDataJson = File.ReadAllText("discordTokenTB.json");
                var discordTokenTBData = JsonConvert.DeserializeObject<string>(discordTokenTBDataJson);

                discordTokenTB.Text = discordTokenTBData;
            }

            canSaveData = true;
        }




        //This prepares the bot list and should happen prior to commands.
        private void InitializeBots()
        {
            // Clear the existing list of bots
            bots.Clear();

            // No checked items
            if (ipListView.CheckedItems.Count < 1)
            {
                UpdateLogger("No IP selected");
                return;
            }

            foreach (ListViewItem item in ipListView.CheckedItems)
            {
                // The IP is assumed to be in the second column (index 2)
                string ip = item.SubItems[2].Text;

                var config = new SwitchConnectionConfig
                {
                    IP = ip,
                    Port = 6000,
                    Protocol = SwitchProtocol.WiFi
                };

                var bot = new Bot(config);
                bots.Add(bot);
            }
        }


        //this is used to add a hold delay to a button. It appends the delay value to the last button press in the macro string
        private void AppendToLastNumberString(string appendText)
        {
            // Get the current text from the textbox
            string text = textBox1.Text.TrimEnd();

            // Split the text into an array of strings
            string[] splitText = text.Split(' ');

            // Check if the last string in the split contains any numbers
            if (Regex.IsMatch(splitText[splitText.Length - 1], @"\d"))
            {
                Console.WriteLine("Not a button");
            }
            else
            {
                // If it does, append the specified text to it
                splitText[splitText.Length - 1] += appendText;
            }

            // Combine the split text back into a single string and put it back in the textbox
            textBox1.Text = string.Join(" ", splitText);
            textBox1.Text += " ";
        }


        //save macro to checklist and write to file
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                ListViewItem newItem = new ListViewItem("");
                newItem.Text = macroNameTB.Text; // Add the name here
                newItem.SubItems.Add(textBox1.Text);
                macroListView.Items.Add(newItem);
                SaveData();
            }

        }


        #region Joycon Button inputs
        //contains all joycon buttons


        //left button because i dont feel like fixing the button name event
        private async void leftbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Left");
            }
            else
            {
                textBox1.AppendText("Left ");
            }
        }

        private async void rightbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Right");
            }
            else
            {
                textBox1.AppendText("Right ");
            }

        }

        private async void ybButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Y");
            }
            else
            {
                textBox1.AppendText("Y ");
            }
        }

        private async void xbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
               await SendLiveButtonPress("X");
            }
            else
            {
                textBox1.AppendText("X ");
            }
        }

        private async void aaBbutton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("A");
            }
            else
            {
                textBox1.AppendText("A ");
            }
        }

        private async void bbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("B");
            }
            else
            {
                textBox1.AppendText("B ");
            }
            
        }

        private async void rtsbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("RStick");
            }
            else
            {
                textBox1.AppendText("RTS ");
            }
        }

        private async void hbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Home");
            }
            else
            {
                textBox1.AppendText("H ");
            }
            
        }
        private async void ssButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Capture");
            }
            else
            {
                textBox1.AppendText("SS ");
            }
        }

        private async void ltsbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("LStick");
            }
            else
            {
                textBox1.AppendText("LTS ");
            }
            
        }

        private async void downbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Down");
            }
            else
            {
                textBox1.AppendText("Down ");
            }
            
        }

        private async void upbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Up");
            }
            else
            {
                textBox1.AppendText("Up ");
            }
            
        }

        private async void rbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Right");
            }
            else
            {
                textBox1.AppendText("R ");
            }
            
        }

        private async void zrbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("ZR");
            }
            else
            {
                textBox1.AppendText("ZR ");
            }
            
        }

        private async void zlbButton_Click_1(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("ZL");
            }
            else
            {
                textBox1.AppendText("ZL ");
            }
        }

        private async void lbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("L");
            }
            textBox1.AppendText("L ");
        }

        private async void zlbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("ZL");
            }
            else
            {
                textBox1.AppendText("ZL ");
            }
        }

        //take the first selected element of the array and move it into textbox
        private void loadButton_Click(object sender, EventArgs e)
        {
            SaveData();
            try
            {
                textBox1.Text = macroListView.SelectedItems[0].SubItems[1].Text;
            }
            catch
            {
                UpdateLogger("No macro selected");
            }
            
        }

        private async void plusbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Plus");
            }
            else
            {
                textBox1.AppendText("+ ");
            }

        }

        private async void minusbButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("Minus");
            }
            else
            {
                textBox1.AppendText("- ");
            }
        }

        #endregion Button

        //hold button seems to be a lot more complicated than i originally thought. Will work more on that soon
        private void holdButton_Click(object sender, EventArgs e)
        {
            AppendToLastNumberString(timerInputField.Text);
            /*
            string timenumdirty = timerInputField.Text;
            string digitsOnly = Regex.Replace(timenumdirty, @"\D", "");
            digitsOnly = digitsOnly + " ";
            textBox1.AppendText(digitsOnly);
            */


        }
        private void timerInputField_TextChanged(object sender, EventArgs e)
        {

        }

        private void delayButton_Click(object sender, EventArgs e)
        {
            if (delayInputField.Text != "")
            {
                string delaynumdirty = delayInputField.Text;
                string delaydigitsOnly = Regex.Replace(delaynumdirty, @"\D", "");
                delaydigitsOnly = "d" + delaydigitsOnly + " ";
                textBox1.AppendText(delaydigitsOnly);
            }
        }

        private void addIpButton_Click(object sender, EventArgs e)
        {
            string ipText = ipTextField.Text;
            string switchName = switchNameTB.Text;

            if (string.IsNullOrEmpty(switchName))
            {
                UpdateLogger("Add a name for the switch");
                return;
            }

            if (!string.IsNullOrEmpty(ipText))
            {
                if (IPAddress.TryParse(ipText, out IPAddress address))
                {
                    ListViewItem existingItem = null;
                    foreach (ListViewItem item in ipListView.Items)
                    {
                        if (item.SubItems[1].Text == switchName && item.SubItems[2].Text == ipText)
                        {
                            existingItem = item;
                            break;
                        }
                    }

                    if (existingItem != null)
                    {
                        existingItem.SubItems[1].Text = switchName;
                        existingItem.SubItems[2].Text = ipText;
                    }
                    else
                    {
                        ListViewItem newItem = new ListViewItem("");  // New ListViewItem for the first column (checkbox)
                        newItem.SubItems.Add(switchName);
                        newItem.SubItems.Add(ipText);
                        ipListView.Items.Add(newItem);
                        ipListView.Invalidate();
                    }

                    ipTextField.Clear();
                    switchNameTB.Clear();
                    SaveData();
                }
                else
                {
                    UpdateLogger("Invalid IP address format.");
                }
            }
        }

        private void deleteIpButton_Click(object sender, EventArgs e)
        {
            if (ipListView.SelectedItems.Count > 0) // Make sure an item is selected
            {
                var selectedItem = ipListView.SelectedItems[0];
                ipListView.Items.Remove(selectedItem);
                SaveData();
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (macroListView.SelectedItems.Count > 0)
            {
                macroListView.Items.Remove(macroListView.SelectedItems[0]);
                SaveData();
            }
            else
            {
                UpdateLogger("No macro selected");
            }
        }

        private CancellationTokenSource cancellationTokenSource;

        private async void playbButton_Click(object sender, EventArgs e)
        {
            bots.Clear();
            //this was the best time to initialize bots.
            InitializeBots();
            if (textBox1.Text == "")
            {
                UpdateLogger("No macro entered");
                return;
            }
            if (loopCheckbox.Checked == true)
            {
                stopbButton.Enabled = true;
                stopbButton.BackColor = Color.Aqua;
                playbButton.Enabled = false;
                UpdateLogger("Starting Macro Loop");
               
            }
            else
            {
                //UpdateLogger("Macro Sent");
            }

            cancellationTokenSource = new CancellationTokenSource(); // Create a new CancellationTokenSource that you call in the stop button code

            string commands = textBox1.Text;
            Func<bool> loopFunc = () => loopCheckbox.Checked;

            foreach (var bot in bots)
                try
                {
                    bot.Connect();
                    await bot.ExecuteCommands(commands, loopFunc, cancellationTokenSource.Token);
                    bot.Disconnect();
                }
                catch (Exception ex)
                {
                  
                    UpdateLogger(ex.Message);
                }


        }

        //stop button terminates the macro loop
        private void stopbButton_Click(object sender, EventArgs e)
        {
            stopbButton.BackColor = Color.White;
            playbButton.Enabled = true;
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel(); // Cancel the CancellationTokenSource. GPT suggested but im still not completely sure how it works.
                UpdateLogger("Stopping Macro");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            holdButton.Enabled = false;
            timerInputField.Enabled = false;
            stopbButton.Enabled = false; 
            
        }

        //log stuff to text box
        public void UpdateLogger(string text)
        {
            if (this.InvokeRequired)  // Check if we are on the UI thread or not
            {
                this.Invoke(new Action<string>(UpdateLogger), text);  // Invoke on the UI thread
            }
            else
            {
                logsBox.Text += (text + Environment.NewLine);
            }
        }

        //live button kill me on this nameing convention...
        private void livebButton_Click(object sender, EventArgs e)
        {
            live = !live;
            string msg;
            if (live)
            {
                msg = "Live mode on";
                livebButton.BackColor = Color.FromArgb(253, 53, 58);
            }
            else
            {
                msg = "Live mode off";
                livebButton.BackColor = Color.White;
            }
            UpdateLogger(msg);
        }

        //Use to send any button to the list of selected IPs
        private async Task SendLiveButtonPress(string button)
        {
            InitializeBots();
            foreach (var bot in bots)
            {
                try
                {
                    bot.Connect();
                    switch (button)
                    {
                        case "A":
                            await bot.PressAButton();
                            break;
                        case "B":
                            await bot.PressBButton();
                            break;
                        case "X":
                            await bot.PressXButton();
                            break;
                        case "Y":
                            await bot.PressYButton();
                            break;
                        case "L":
                            await bot.PressLButton();
                            break;
                        case "R":
                            await bot.PressRButton();
                            break;
                        case "ZL":
                            await bot.PressZLButton();
                            break;
                        case "ZR":
                            await bot.PressZRButton();
                            break;
                        case "Up":
                            await bot.PressDPadUp();
                            break;
                        case "Down":
                            await bot.PressDPadDown();
                            break;
                        case "Left":
                            await bot.PressDPadLeft();
                            break;
                        case "Right":
                            await bot.PressDPadRight();
                            break;
                        case "Plus":
                            await bot.PressPlusButton();
                            break;
                        case "Minus":
                            await bot.PressMinusButton();
                            break;
                        case "Home":
                            await bot.PressHomeButton();
                            break;
                        case "Capture":
                            await bot.PressCaptureButton();
                            break;
                        case "LStick":
                            await bot.PressLeftStickButton();
                            break;
                        case "RStick":
                            await bot.PressRightStickButton();
                            break;
                        
                        
                        // Add cases for the other buttons as needed...
                        default:
                            throw new ArgumentException("Invalid button string.");
                    }
                    bot.Disconnect();
                }
                catch (Exception ex)
                {
                    UpdateLogger(ex.Message);
                }
            }
        }

        private async void botStartBButton_Click(object sender, EventArgs e)
        {
            SaveData();
            _bot?.StopAsync().Wait();  // Stop existing bot if any. Wait for completion.

            // Create lists to store the user IDs and channel IDs
            List<ulong> userIds = new List<ulong>();
            List<ulong> channelIds = new List<ulong>();

            foreach (ListViewItem item in userIDLV.Items)
            {
                if (ulong.TryParse(item.SubItems[1].Text, out ulong parsedUserId))
                {
                    userIds.Add(parsedUserId);
                    UpdateLogger($"Added user ID: {parsedUserId}");
                }
                else
                {
                    UpdateLogger($"Failed to parse user ID: {item.SubItems[1].Text}");
                }
            }

            // For Channel IDs
            foreach (ListViewItem item in channelIDLV.Items)
            {
                if (ulong.TryParse(item.SubItems[1].Text, out ulong parsedChannelId))
                {
                    channelIds.Add(parsedChannelId);
                    UpdateLogger($"Added channel ID: {parsedChannelId}");
                }
                else
                {
                    UpdateLogger($"Failed to parse channel ID: {item.SubItems[1].Text}");
                }

                // Assuming item.SubItems[2].Text contains channel IDs
                if (ulong.TryParse(item.SubItems[1].Text, out parsedChannelId))
                {
                    channelIds.Add(parsedChannelId);
                    UpdateLogger($"Added channel ID: {parsedChannelId}");
                }
                else
                {
                    UpdateLogger($"Failed to parse channel ID: {item.SubItems[1].Text}");
                }
            }

            _bot = new DiscordBot(discordTokenTB.Text,userIds,channelIds,ipDict,macroDict);  // Initialize a new instance
            _bot.LogAction = UpdateLogger;
            await _bot.MainAsync();
            botStartBButton.Enabled = false;
            botStartBButton.Visible = false;
        }

        private async void botStopBButton_Click(object sender, EventArgs e)
        {
            await _bot?.StopAsync();  // Stop the bot if it exists
            botStartBButton.Enabled = true;
            botStartBButton.Visible = true;
        }

        private void discordTokenTB_TextChanged(object sender, EventArgs e)
        {
            SaveData();
        }

        private void userAddButton_Click(object sender, EventArgs e)
        {
            // Create a new ListViewItem
            ListViewItem item = new ListViewItem(userNameTB.Text);
            item.SubItems.Add(UserIDTB.Text);

            // Add the ListViewItem to the ListView
            userIDLV.Items.Add(item);
            UserIDTB.Text = "";
            userNameTB.Text = "";
            SaveData();
        }


        private void userDeleteButton_Click(object sender, EventArgs e)
        {
            // Check if an item is selected
            if (userIDLV.SelectedItems.Count > 0)
            {
                // Remove the selected item
                userIDLV.Items.Remove(userIDLV.SelectedItems[0]);
            }
            SaveData();
        }


        private void ciAddButton_Click(object sender, EventArgs e)
        {
            // Create a new ListViewItem
            ListViewItem item = new ListViewItem(channelNameTB.Text);
            item.SubItems.Add(channelIDTB.Text);

            // Add the ListViewItem to the ListView
            channelIDLV.Items.Add(item);
            channelNameTB.Text = "";
            channelIDTB.Text = "";
            SaveData();
        }


        private void ciDeleteButton_Click(object sender, EventArgs e)
        {
            // Check if an item is selected
            if (channelIDLV.SelectedItems.Count > 0)
            {
                // Remove the selected item
                channelIDLV.Items.Remove(channelIDLV.SelectedItems[0]);
            }
            SaveData();
        }

    }
}
