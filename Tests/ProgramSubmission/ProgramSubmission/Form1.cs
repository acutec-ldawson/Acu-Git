using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AcuGitLibrary;

namespace ProgramSubmission
{
    public partial class Form1 : Form
    {
        private string subPath = null;
        private bool setup = false;
        private string masterRoot = @"C:\Users\ldawson\TestFolder2\JobFiles";
        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            setup = checkBox1.Checked;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (subPath != null && textBox2.Text != "") {
                ChangeRequest request = new ChangeRequest(subPath, masterRoot, ChangeRequest.RequestType.Job_Based, false,textBox2.Text);
                request.Submit();
                label10.Text = "Program Submitted ...";

                textBox1.Text = null;
                textBox2.Text = null;
                textBox3.Text = null;
                textBox4.Text = null;
                textBox5.Text = null;
                textBox6.Text = null;
                textBox7.Text = null;
                textBox8.Text = null;
                textBox9.Text = null;
                textBox10.Text = null;
                textBox11.Text = null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                subPath = folderBrowserDialog1.SelectedPath;
                textBox10.Text = subPath;
            }
        }


        //Accidental made events, don't remove -- breaks form
        //*************************************************************
        private void button3_Click(object sender, EventArgs e)
        {
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
        }
        private void label7_Click(object sender, EventArgs e)
        {
        }
        private void label9_Click(object sender, EventArgs e)
        {
        }
        //*************************************************************
    }
}
