using Newtonsoft.Json;
using SysBot.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;





namespace SysbotMacro
{
    public partial class Form1 : Form
    {
        private List<Bot> bots = new List<Bot>(); // Declare the bots list

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }

        private void SaveData()
        {
            var ipListData = ipList.Items.Cast<string>().ToList();
            var saveCheckListData = saveCheckList.Items.Cast<string>().ToList();

            File.WriteAllText("ipList.json", JsonConvert.SerializeObject(ipListData));
            File.WriteAllText("saveCheckList.json", JsonConvert.SerializeObject(saveCheckListData));
        }

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
                    Port = 6000, // replace with your Switch's port
                    Protocol = SwitchProtocol.WiFi // set the protocol to WiFi
                };

                var bot = new Bot(config);
                bots.Add(bot);
            }
        }


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


        //save macro
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                saveCheckList.Items.Add(textBox1.Text);
                textBox1.Clear();
                SaveData();
            }

        }

        //left button
        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("Left ");
        }








        private void yButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("Y ");
        }

        private void xButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("X ");
        }

        private void aButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("A ");
        }

        private void bButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("B ");
        }

        private void rtsButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("RTS ");
        }

        private void homeButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("H ");
        }

        private void ssButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("SS ");
        }

        private void ltsButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("LTS ");
        }

        private void downbutton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("Down ");
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("Up ");
        }

        private void rButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("R ");
        }

        private void zrButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("ZR ");
        }

        private void lButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("L ");
        }

        private void zlButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("ZL ");
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            SaveData();
            textBox1.Text = saveCheckList.CheckedItems[0].ToString();
        }

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

        private void rightButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("Right ");
        }

        private async void debugSysbotBaseButton_Click(object sender, EventArgs e)
        {
            foreach (var bot in bots)
            {
                bot.Connect();
                await bot.PressHomeButton();
                bot.Disconnect();
            }
            //InitializeBots(); // Re-initialize the bots after disconnecting
        }

        private void addIpButton_Click(object sender, EventArgs e)
        {
            if (ipTextField.Text != "")
            {
                ipList.Items.Add(ipTextField.Text);
                ipTextField.Clear();
                //InitializeBots();
                SaveData();
            }
        }

        private void deleteIpButton_Click(object sender, EventArgs e)
        {
            if (ipList.SelectedIndex != -1) // Make sure there is a selected item to delete
            {
                ipList.Items.RemoveAt(ipList.SelectedIndex);
                //InitializeBots();
                SaveData();
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            saveCheckList.Items.RemoveAt(saveCheckList.SelectedIndex);
            //InitializeBots();
            SaveData();

        }

        private void plusButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("+ ");
        }

        private void minusButton_Click(object sender, EventArgs e)
        {
            textBox1.AppendText("- ");
        }

        private CancellationTokenSource cancellationTokenSource;

        private async void playButton_Click(object sender, EventArgs e)
        {
            bots.Clear();
            InitializeBots();
            if (textBox1.Text == "")
            {
                UpdateLogger("No macro loaded");
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

            cancellationTokenSource = new CancellationTokenSource(); // Create a new CancellationTokenSource

            string commands = textBox1.Text;
            Func<bool> loopFunc = () => loopCheckbox.Checked; // replace with your actual loop checkbox

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

            //InitializeBots(); // Re-initialize the bots after disconnecting

        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            stopButton.BackColor = Color.White;
            playButton.Enabled = true;
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel(); // Cancel the CancellationTokenSource
                UpdateLogger("Stopping Macro");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            holdButton.Enabled = false;
            timerInputField.Enabled = false;
            stopButton.Enabled = false; 
            
        }

        public void UpdateLogger(string text)
        {
            logsBox.Text += (text + Environment.NewLine);
        }
    }
}
