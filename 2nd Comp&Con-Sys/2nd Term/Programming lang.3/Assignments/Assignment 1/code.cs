using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        int i = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 500;
            timer1.Enabled = true;
            trackBar1.Value = 3;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 100;
            timer1.Enabled = true;
            trackBar1.Value = 7;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 50;
            timer1.Enabled = true;
            trackBar1.Value = 10;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {            
            pictureBox1.Image = imageList1.Images[i];
            i++;
            if (i >= 11) i = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
            trackBar1.Value = 0;
            label1.Text = Convert.ToString(trackBar1.Value);
            pictureBox1.Image = imageList1.Images[i];
            timer1.Enabled = false;

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = Convert.ToString(trackBar1.Value);

            timer1.Interval = -(trackBar1.Value*50) + 550;
            timer1.Enabled = true;
            if (trackBar1.Value > 0) {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = Convert.ToString(trackBar1.Value);
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
        }
    }
}
