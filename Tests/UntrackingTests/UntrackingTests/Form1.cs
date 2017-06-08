using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AcuGitLibrary;

namespace UntrackingTests
{
    public partial class Form1 : Form
    {
        string root = null;
        string fileName = null;
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //File selection
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    fileName = openFileDialog1.FileName;
                    textBox1.Text = fileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //simply to learn more about c# random number generator
            int count = 0;
            Random R = new Random(344);
            for (int i = 1; i <= 1000; i++)
            {
                if (R.NextDouble() < 0.250)
                {
                    count++;
                }
            }
            textBox3.Text = count.ToString();

            //Go to repo
            //Git git = new Git(root);
            //git.Untrack(fileName);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                root = folderBrowserDialog1.SelectedPath;
                textBox4.Text = root;
            }
        }
        //////////////////////////////////////////////////////////////
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
