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

namespace CherryPickLibraryTests
{
    public partial class Form1 : Form
    {
        string root = @"C:\Users\ldawson\TestFolder \Stuff";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawGraph();
        }
        private void DrawGraph() {
            int top = 10;
            int left = 10;
            List<string> branchnames = new List<string>();
            Git git = new Git(root);
            git.CommitList();
            foreach (CommitInfo info in git.Commitlist) {
                if (!branchnames.Contains(info.BranchName)) branchnames.Add(info.BranchName);
            }
            foreach (string name in branchnames) {
                Label BranchLabel = new Label();
                BranchLabel.Left = left;
                BranchLabel.Top = top;
                this.Controls.Add(BranchLabel);
                BranchLabel.Text = (name);
                top += BranchLabel.Height + 3;
                foreach (CommitInfo info in git.Commitlist) {
                    if (info.BranchName == name)
                    {
                        Button button = new Button();
                        button.Left = left;
                        button.Top = top;
                        button.Text = info.CommitShortMessage;
                        this.Controls.Add(button);

                        ButtonEventArgs evnt = new ButtonEventArgs(info, name);
                        button.Click += (sender, e) => ButtonHandler(button, evnt);
                        top += BranchLabel.Height + 3;
                    }
                }
                left += 80;
                top = 10;
            }
            git.Checkout("master");
        }
        private void ButtonHandler(object sender, EventArgs e) {
            ButtonEventArgs evnt = (ButtonEventArgs)e;
            Git git = new Git(root);
            git.CommitPick(evnt.CINF);
            //DrawGraph();
        }
    }
    public class ButtonEventArgs : EventArgs
    {
        public CommitInfo CINF;
        public string BranchName;
        public ButtonEventArgs(CommitInfo _cinf, string branchname)
        {
            BranchName = branchname;
            CINF = _cinf;
        }
    }
}
