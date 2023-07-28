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





namespace SysbotMacro
{
    public partial class Form1 : Form
    {
        // Declare the bots list that will be looped through when commands are sent.
        private List<Bot> bots = new List<Bot>(); 

        // Used for Live button pressing
        private bool live;




        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        // Store the IP's and the Macros in each list when changes are made. Keep data after the program closes
        private void SaveData()
        {
            var ipListData = ipList.Items.Cast<string>().ToList();
            var saveCheckListData = saveCheckList.Items.Cast<string>().ToList();

            File.WriteAllText("ipList.json", JsonConvert.SerializeObject(ipListData));
            File.WriteAllText("saveCheckList.json", JsonConvert.SerializeObject(saveCheckListData));
        }

        // Loads the stored data back into the lists
        private void LoadData()
        {
            if (File.Exists("ipList.json"))
            {
                var ipListData = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("ipList.json"));
                ipList.Items.AddRange(ipListData.ToArray());
            }

            if (File.Exists("saveCheckList.json"))
            {
                var saveCheckListData = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText("saveCheckList.json"));
                saveCheckList.Items.AddRange(saveCheckListData.ToArray());
            }
        }

        //This prepares the bot list and should happen prior to commands.
        private void InitializeBots()
        {
            if (ipList.CheckedItems.Count < 1)
            {
                // Do nothing if no IPs are checked
                UpdateLogger("No IP selected");
                return;
            }
            bots.Clear(); // Clear the existing list of bots
            foreach (string ip in ipList.CheckedItems)
            {
                var config = new SwitchConnectionConfig
                {
                    IP = ip, // Set the IP from the ipList
                    Port = 6000,
                    Protocol = SwitchProtocol.WiFi // set the protocol to WiFi. Maybe USB version coming soon >.>
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
            if (textBox1.Text != "")
            {
                saveCheckList.Items.Add(textBox1.Text);
                SaveData();

            }

        }


        #region Joycon Button inputs
        //contains all joycon buttons


        //left button because i dont feel like fixing the button name event
        private async void button7_Click(object sender, EventArgs e)
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

        private async void rightButton_Click(object sender, EventArgs e)
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

        private async void yButton_Click(object sender, EventArgs e)
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

        private async void xButton_Click(object sender, EventArgs e)
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

        private async void aButton_Click(object sender, EventArgs e)
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

        private async void bButton_Click(object sender, EventArgs e)
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

        private async void rtsButton_Click(object sender, EventArgs e)
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

        private async void homeButton_Click(object sender, EventArgs e)
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

        private async void ltsButton_Click(object sender, EventArgs e)
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

        private async void downbutton_Click(object sender, EventArgs e)
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

        private async void upButton_Click(object sender, EventArgs e)
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

        private async void rButton_Click(object sender, EventArgs e)
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

        private async void zrButton_Click(object sender, EventArgs e)
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

        private async void lButton_Click(object sender, EventArgs e)
        {
            if (live)
            {
                await SendLiveButtonPress("L");
            }
            textBox1.AppendText("L ");
        }

        private async void zlButton_Click(object sender, EventArgs e)
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
                textBox1.Text = saveCheckList.CheckedItems[0].ToString();
            }
            catch
            {
                UpdateLogger("No macro selected");
            }
            
        }

        private async void plusButton_Click(object sender, EventArgs e)
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

        private async void minusButton_Click(object sender, EventArgs e)
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

        

        private async void debugSysbotBaseButton_Click(object sender, EventArgs e)
        {
            foreach (var bot in bots)
            {
                bot.Connect();
                await bot.PressHomeButton();
                bot.Disconnect();
            }
        }

        private void addIpButton_Click(object sender, EventArgs e)
        {
            if (ipTextField.Text != "")
            {
                ipList.Items.Add(ipTextField.Text);
                ipTextField.Clear();
                SaveData();
            }
        }

        private void deleteIpButton_Click(object sender, EventArgs e)
        {
            if (ipList.SelectedIndex != -1) // Make sure there is a selected item to delete
            {
                ipList.Items.RemoveAt(ipList.SelectedIndex);
                SaveData();
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            saveCheckList.Items.RemoveAt(saveCheckList.SelectedIndex);
            SaveData();

        }

        private CancellationTokenSource cancellationTokenSource;

        private async void playButton_Click(object sender, EventArgs e)
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
                stopButton.Enabled = true;
                stopButton.BackColor = Color.Aqua;
                playButton.Enabled = false;
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
        private void stopButton_Click(object sender, EventArgs e)
        {
            stopButton.BackColor = Color.White;
            playButton.Enabled = true;
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
            stopButton.Enabled = false; 
            
        }

        //log stuff to text box
        public void UpdateLogger(string text)
        {
            logsBox.Text += (text + Environment.NewLine);
        }

        //live button kill me on this nameing convention...
        private void button1_Click_1(object sender, EventArgs e)
        {
            live = !live;
            string msg;
            if (live)
            {
                msg = "Live mode on";
                liveModeButton.BackColor = Color.Aqua;
            }
            else
            {
                msg = "Live mode off";
                liveModeButton.BackColor = Color.White;
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

        //launch info window
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 Form2 = new Form2();
            Form2.Show();
        }

    }
}
