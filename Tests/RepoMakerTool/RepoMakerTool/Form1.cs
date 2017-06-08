using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AcuGitLibrary;

namespace RepoMakerTool
{
    public partial class Form1 : Form
    {
        bool includeSubs = false;
        bool includeTop = false;
        bool overwrite = false;
        bool defIgnore = false;
        string path = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            includeTop = checkBox2.Checked;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            includeSubs = checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                path = folderBrowserDialog1.SelectedPath;
                textBox1.Text = path;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (path != null)
            {
                RepoMaker repomaker = new RepoMaker(path, includeSubs, includeTop,overwrite, defIgnore);
                repomaker.Run();
                textBox1.Text = null;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            overwrite = checkBox3.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            defIgnore = checkBox4.Checked;
        }
    }
}
