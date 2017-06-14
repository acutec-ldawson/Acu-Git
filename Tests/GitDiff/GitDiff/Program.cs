using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;
using System.IO;
using AcuGitLibrary;

namespace GitDiff
{
    class Program
    {
        static void Main(string[] args)
        {
            Git_Diff();
        }
        static void Git_Diff() {
            string repoPath = @"\\acutec.local\Acutec\Network\User Folders\ldawson\Desktop\DiffTestEnv\";
            string[] Files = Directory.GetFiles(repoPath);
            Console.WriteLine("Select a File: \n");
            for (int i = 0; i < Files.Length; i++) {
                Console.WriteLine("[{0}] {1}", i.ToString(), Path.GetFileName(Files[i]));
            }
            int choice = Convert.ToInt32(Console.ReadLine());
            string filename = Path.GetFileName(Files[choice]);
            using (var repo = new Repository(repoPath)) {
                List<Branch> branches = new List<Branch>();
                Console.WriteLine("Select Branch to compare to: ");
                int bChoice = 0;
                foreach (Branch b in repo.Branches) {
                    branches.Add(b);
                    Console.WriteLine("[{0}] {1}", branches.Count-1, b.FriendlyName);
                }
                bChoice = Convert.ToInt32(Console.ReadLine());
                string branchname = branches[bChoice].FriendlyName;
                Git git = new Git(repoPath);
                string[] response = git.Diff(filename, branchname);
                foreach (string s in response) {
                    Console.WriteLine(s);
                }
                Console.Read();
            }
        }
    }
}
