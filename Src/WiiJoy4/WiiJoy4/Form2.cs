using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace WiiJoy4
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public static Form1 form1 = new Form1();
        public void SetLabel1Text(string text)
        {
            this.label1.Text = text;
        }
        public void SetLabel2Text(string text)
        {
            this.label2.Text = text;
        }
        public void SetPictureBox(Bitmap EditableImg)
        {
            this.pictureBox1.Image = EditableImg;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            this.Location = new System.Drawing.Point(50, 50);
        }
        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            form1.Setgetstate();
            this.Hide();
            e.Cancel = true;
        }
    }
}
