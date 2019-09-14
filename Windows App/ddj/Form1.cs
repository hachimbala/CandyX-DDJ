using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Globalization;
using KnobControl;

namespace ddj
{
    public partial class Form1 : Form
    {
        static SerialPort serialPort1;
        int red, green, blue, pad_number;
        int knob1, knob2, knob3, knob4, knob5, knob6;
        string indata;

        public Form1()
        {
            InitializeComponent();
            getAvailablePorts();
            serialPort1 = new SerialPort();
            serialPort1.ReadTimeout = 500;
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuMetroTextbox1_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton1_Click_1(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(tabPage1);
        }

        private void bunifuFlatButton2_Click_1(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(tabPage2);
        }

        void getAvailablePorts()
        {
            String[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
        }

        private void knobControl1_ValueChanged(object Sender)
        {

        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if(comboBox1.Text == "")
                {
                    textBox1.ForeColor = Color.Red;
                    textBox1.Text = "Please select a Serial Port";
                    button2.Enabled = false;
                    bunifuFlatButton4.Enabled = false;
                }
                else
                {
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = 9600;
                    serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    serialPort1.Open();
                    button2.Enabled = true;
                    bunifuFlatButton3.Enabled = false;
                    bunifuFlatButton4.Enabled = true;
                    textBox1.ForeColor = Color.Green;
                    textBox1.Text = "Connected Succesfuly!";
                }
            }
            catch (UnauthorizedAccessException)
            {
                textBox1.ForeColor = Color.Red;
                textBox1.Text = "Unauthorized Access";
            }
        }

        private void ThreadSafe(Action callback, Form form)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.RunWorkerCompleted += (obj, e) =>
            {
                if (form.InvokeRequired)
                    form.Invoke(callback);
                else
                    callback();
            };
            worker.RunWorkerAsync();
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            indata = sp.ReadLine();
            string[] words = indata.Split(',');
            try
            {
                Int32.TryParse(words[0], out knob1);
                Int32.TryParse(words[1], out knob2);
                Int32.TryParse(words[2], out knob3);
                Int32.TryParse(words[3], out knob4);
                Int32.TryParse(words[4], out knob5);
                Int32.TryParse(words[5], out knob6);
            }
            catch (Exception)
            {

            }

            //Lo llamas de esta manera
            ThreadSafe(() =>
            {
                arcScaleComponent1.Value = knob1;
                arcScaleComponent2.Value = knob2;
                arcScaleComponent3.Value = knob3;
                arcScaleComponent4.Value = knob4;
                arcScaleComponent5.Value = knob5;
                arcScaleComponent6.Value = knob6;
                knobControl1.Value = knob1;
                knobControl2.Value = knob2;
                knobControl3.Value = knob3;
                knobControl4.Value = knob4;
                knobControl5.Value = knob5;
                knobControl6.Value = knob6;
            }, this); 
        }
        
        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            button2.Enabled = false;
            bunifuFlatButton4.Enabled = false;
            bunifuFlatButton3.Enabled = true;
            textBox1.ForeColor = Color.Blue;
            textBox1.Text = "Disconnected Succesfuly!";
        }

        private void bunifuFlatButton6_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "5";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 21;
        }

        private void bunifuSlider2_ValueChanged(object sender, EventArgs e)
        {
            green = bunifuSlider2.Value;
            button1.BackColor = Color.FromArgb(255, red, green, blue);
        }

        private void bunifuSlider3_ValueChanged(object sender, EventArgs e)
        {
            blue = bunifuSlider3.Value;
            button1.BackColor = Color.FromArgb(255, red, green, blue);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void bunifuTextBox4_TextChange(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton7_Click(object sender, EventArgs e)
        {

            bunifuTextBox4.Text = "2";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 24;
        }

        private void bunifuFlatButton9_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "3";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 23;
        }

        private void bunifuFlatButton11_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "4";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 22;
        }

        private void bunifuFlatButton8_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "6";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 20;
        }

        private void bunifuFlatButton10_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "7";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 19;
        }

        private void bunifuFlatButton12_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "8";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 18;
        }

        private void bunifuFlatButton13_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "9";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 8;
        }

        private void bunifuFlatButton15_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "10";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 7;
        }

        private void bunifuFlatButton17_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "11";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 6;
        }

        private void bunifuFlatButton19_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "12";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 5;
        }

        private void bunifuFlatButton14_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "13";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 4;
        }

        private void bunifuFlatButton16_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "14";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 3;
        }

        private void bunifuFlatButton18_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "15";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 2;
        }

        private void bunifuFlatButton20_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "16";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(pad_number >= 1 && pad_number <= 48)
            {
                serialPort1.Write(pad_number + "," + green + "," + red + "," + blue);
                textBox2.ForeColor = Color.Green;
                textBox2.Text = "Data sent!";
            }
            else
            {
                textBox2.ForeColor = Color.Red;
                textBox2.Text = "Select a PAD";
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void bunifuRadioButton1_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "9";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 30;
        }

        private void bunifuRadioButton2_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "14";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 13;
        }

        private void bunifuFlatButton45_Click(object sender, EventArgs e)
        {
            bunifuPages1.SetPage(tabPage3);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton38_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "1";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 34;
        }

        private void bunifuFlatButton39_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "2";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 33;
        }

        private void bunifuFlatButton41_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "3";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 32;
        }

        private void bunifuFlatButton44_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "4";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 31;
        }

        private void bunifuFlatButton37_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "5";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 17;
        }

        private void bunifuFlatButton42_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "6";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 16;
        }

        private void bunifuFlatButton43_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "7";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 15;
        }

        private void bunifuFlatButton40_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "8";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 14;
        }

        private void bunifuFlatButton29_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "10";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 26;
        }

        private void bunifuFlatButton30_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "11";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 27;
        }

        private void bunifuFlatButton32_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "12";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 28;
        }

        private void bunifuFlatButton31_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "13";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 29;
        }

        private void bunifuFlatButton36_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "15";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 31;
        }

        private void bunifuFlatButton35_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "16";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 32;
        }

        private void bunifuFlatButton34_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "17";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 33;
        }

        private void bunifuFlatButton33_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "18";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 34;
        }

        private void bunifuFlatButton7_Click_1(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "19";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 29;
        }

        private void bunifuFlatButton23_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "20";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 28;
        }

        private void bunifuFlatButton25_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "21";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 27;
        }

        private void bunifuFlatButton28_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "22";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 26;
        }

        private void bunifuFlatButton5_Click_1(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "23";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 12;
        }

        private void bunifuFlatButton26_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "24";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 11;
        }

        private void bunifuFlatButton27_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "25";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 10;
        }

        private void bunifuFlatButton24_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "26";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "BUTTON:";
            pad_number = 9;
        }

        private void bunifuTextBox8_TextChange(object sender, EventArgs e)
        {

        }

        private void bunifuTextBox9_TextChange(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton46_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/hachimbala");
        }

        private void bunifuFlatButton47_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCnf4IQqt3SaaW3gxyrZJAqQ");
        }

        private void bunifuFlatButton48_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.youtube.com/channel/UCnf4IQqt3SaaW3gxyrZJAqQ");
        }

        private void bunifuFlatButton48_Click_1(object sender, EventArgs e)
        {
            getAvailablePorts();
        }

        private void bunifuTextBox3_TextChange(object sender, EventArgs e)
        {
            
        }

        private void bunifuTextBox10_TextChange(object sender, EventArgs e)
        {

        }

        private void bunifuTextBox4_TextChange_1(object sender, EventArgs e)
        {

        }

        private void knobControl1_Load(object sender, EventArgs e)
        {

        }

        private void knobControl1_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "1";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "POTENTIOMETER:";
            pad_number = 43;
        }

        private void bunifuTextBox11_TextChange(object sender, EventArgs e)
        {

        }

        private void bunifuGauge1_Load(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuTextBox5_TextChange(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
            bunifuTextBox4.Text = "1";
            bunifuTextBox10.ForeColor = Color.Blue;
            bunifuTextBox10.Text = "PAD:";
            pad_number = 25;
        }

        private void bunifuSlider1_ValueChanged(object sender, EventArgs e)
        {
            red = bunifuSlider1.Value;
            button1.BackColor = Color.FromArgb(255, red, green, blue);
        }
    }
}
