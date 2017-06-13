using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LibGit2Sharp;
using System.Net.Mail;

namespace AcuGitLibrary
{
    /// <summary>
    /// <c>RepoMaker</c> is a class that can be used to quickly make git repositories
    /// </summary>
    public class RepoMaker
    {

        /// <summary>
        /// The rootpath is the top directory path that will become a repository, or where repositories will be made
        /// </summary>
        public string root = null;
        /// <summary>
        /// Include the subdirectories of the folders within the rootpath
        /// </summary>
        public bool includeSubs = false;
        /// <summary>
        /// Make the rootpath directory into a repository as well
        /// </summary>
        public bool singleDirectory = false;
        /// <summary>
        /// Dictates whether or not the RepoMaker will erase older .git folders (Important Note: If this is false I don't believe the program will function correctly)
        /// </summary>
        public bool overwrite = true;
        /// <summary>
        /// Determines whether or not the program will create the default .gitignore file that will ignore (.db, .pdf, .doc, .docx, .xls, .xlsx, .tmp) file types
        /// </summary>
        public bool setDefaultIgnore = true;
        /// <summary>
        /// Simplest constructor of the RepoMaker
        /// </summary>
        /// <param name="_rootpath"> The rootpath is the top directory path that will become a repository, or where repositories will be made</param>
        /// <param name="_override">Erases any pre-existing .git folders</param>
        /// <param name="_setDefaultIgnore">Creates a .gitignore that ignores (.db, .pdf, .doc, .docx, .xls, .xlsx, .tmp) file types </param>
        public RepoMaker(string _rootpath, bool _override = true, bool _setDefaultIgnore = true)
        {
            //Simplest constructor
            root = _rootpath;
            overwrite = _override;
        }
        /// <summary>
        /// Second constructor
        /// </summary>
        /// <param name="_rootpath">The rootpath is the top directory path that will become a repository, or where repositories will be made</param>
        /// <param name="_includeSubs">Include the subdirectories of the folders within the rootpath</param>
        /// <param name="_override">Erases any pre-existing .git folders</param>
        ///  <param name="_setDefaultIgnore">Creates a .gitignore that ignores (.db, .pdf, .doc, .docx, .xls, .xlsx, .tmp) file types </param>
        public RepoMaker(string _rootpath, bool _includeSubs, bool _override = true, bool _setDefaultIgnore = true)
        {
            root = _rootpath;
            includeSubs = _includeSubs;
            overwrite = _override;
        }
        /// <summary>
        /// Third constructor
        /// </summary>
        /// <param name="_rootpath">The rootpath is the top directory path that will become a repository, or where repositories will be made</param>
        /// <param name="_includeSubs">Include the subdirectories of the folders within the rootpath</param>
        /// <param name="_singleDirectory">Make the rootpath directory into a repository as well</param>
        /// <param name="_override">Erases any pre-existing .git folders</param>
        ///  <param name="_setDefaultIgnore">Creates a .gitignore that ignores (.db, .pdf, .doc, .docx, .xls, .xlsx, .tmp) file types </param>
        public RepoMaker(string _rootpath, bool _includeSubs, bool _singleDirectory, bool _override = true, bool _setDefaultIgnore = true)
        {
            root = _rootpath;
            includeSubs = _includeSubs;
            singleDirectory = _singleDirectory;
            overwrite = _override;
        }
        /// <summary>
        /// <c>Run()</c> is the method in the <c>RepoMaker</c> Class <para>that will begin creating the repositories in accordance with the parameters that have been entered</para>
        /// </summary>
        public void Run() {
            //Makes the repository as according to the constructor
            if (root != null) {
                if (singleDirectory) {
                    Git git = new Git(root);
                    if (overwrite) {
                        if (System.IO.Directory.Exists(root + @"\.git\"))
                        {
                            UberDel(root + @"\.git\");
                            git.ResetIgnore();
                        }
                    }
                    git.Init();
                    if (setDefaultIgnore)
                    {
                        string[] ignoreDefaults = { "*.db", "*.pdf", "*.doc", "*.docx", "*.xls", "*.xlsx", "*.tmp" };
                        git.CreateIgnore();
                        git.AddIgnore(ignoreDefaults);
                        git.Stage();
                        git.Commit();
                    }
                    git.Stage();                                          //git add -- stage all the files contained within the repository
                    git.Commit("Initial Commit <Automated>");             //git commit -- do the initial commit of the repository
                }
                string[] Dirs;
                if (includeSubs)
                {
                    Dirs = System.IO.Directory.GetDirectories(root, "*", SearchOption.AllDirectories);
                }
                else {
                    Dirs = System.IO.Directory.GetDirectories(root, "*", SearchOption.TopDirectoryOnly);
                }
                for (int i = 0; i < Dirs.Length; i++)
                {
                    Git git = new Git(Dirs[i]);
                    if (overwrite)
                    {
                        if (System.IO.Directory.Exists(Dirs[i] + @"\.git\"))
                        {
                            UberDel(Dirs[i] + @"\.git\");
                            git.ResetIgnore();
                        }
                    }
                    git.Init();
                    if (setDefaultIgnore)
                    {
                        string[] ignoreDefaults = { "*.db", "*.pdf", "*.doc", "*.docx", "*.xls", "*.xlsx", "*.tmp" };
                        git.CreateIgnore();
                        git.AddIgnore(ignoreDefaults);
                        git.Stage();
                        git.Commit();
                    }
                    git.Stage();                                          //git add -- stage all the files contained within the repository
                    git.Commit("Initial Commit <Automated>");             //git commit -- do the initial commit of the repository
                }
            }
        }
        /// <summary>
        /// Most assured way to delete .git directories
        /// For whatever reason the basic Directory.Delete() didn't work
        /// This method is more thorough
        /// </summary>
        /// <param name="_path"> The path to the directory that you wish to delete</param>
        private void UberDel(string _path) {

            //System.IO.Directory.SetAccessControl(_path, new System.Security.AccessControl.DirectorySecurity());

            string[] files = System.IO.Directory.GetFiles(_path, "*", SearchOption.AllDirectories);
            string[] directories = System.IO.Directory.GetDirectories(_path, "*", SearchOption.AllDirectories);
            foreach (string file in files) {
                System.IO.File.SetAttributes(file, FileAttributes.Normal);
                System.IO.File.Delete(file);
            }
            foreach (string dir in directories) {
                try
                {
                    System.IO.Directory.Delete(dir, true);
                }
                catch (DirectoryNotFoundException dire) { }
            }
            System.IO.Directory.Delete(_path, true);
        }
    }
    /// <summary>
    /// Used to submit new files, or change requests, to a master repository 
    /// </summary>
    public class ChangeRequest
    {

        /*******************************************************************
         *      Aspects of this code (specifically those pretaining to 
         * job# based submission) could be made easier by either hard coding 
         * in the root directory of where the job files are stored, or setting
         * a default
         *///***************************************************************

        /// <summary>
        /// Indicates the method by which the user wants to submit the job
        /// </summary>
        public enum RequestType
        {
            /// <summary>
            /// Job based submissions will utilize the job number and path to the master's root i.e. U:\CNC Files ...
            /// </summary>
            Job_Based,
            /// <summary>
            /// Requires the user to enter the exact path of the master repository and the temp file's pathway as well
            /// </summary>
            Precise
        }
        RequestType reqType = RequestType.Precise;

        string subPath = null;
        string repoPath = null;
        string masterRoot = null;

        string[] files = null;
        bool[] fileDiffs;
        string[] repoMatch;
        bool deleteSub = false;

        private string submitterName = "(Automated)";

        string jobNumb;


        /// <summary>
        ///Request Constructor: 
        /// <para>By default the program will ... </para>
        /// <para>  ... Not delete the submitted files</para>
        /// <para>  ... Run RequestType.Precise</para>
        /// <para>  ... Run SubmissionType.Single</para>
        /// <para> !! IMPORTANT !!:</para>
        /// <para>  The JobBased submission will not work unless the masterRoot is set to a value other than <c>null</c></para>
        /// </summary>
        /// <param name="_subPath"><c>string</c>: The path to either ...<para><c>SubmissionType.Single</c>: The file that is being submitted</para><para><c>SubmissionType.Multi</c>: The folder that contains the files being submitted</para></param>
        /// <param name="_repoPath"><c>string</c>: The path to either ...<para><c>RequestType.Precise</c>: The path to the folder that contains the file</para><para><c>RequestType.JobBased</c>: The root folder that contains the jobs folders</para></param>
        /// <param name="_jobNumb">The job number for the submission<para>  Required for JobBased submission</para></param>
        /// <param name="_reqType"><c>RequestType</c>: set to <c>RequestType.Precise</c> by default</param>
        /// <param name="_deleteSub"></param>
        public ChangeRequest(string _subPath, string _repoPath, RequestType _reqType = RequestType.Precise, bool _deleteSub = false, string _jobNumb = null)
        {
            reqType = _reqType;
            subPath = _subPath;
            repoPath = _repoPath;
            deleteSub = _deleteSub;
            jobNumb = _jobNumb;

            if (reqType == RequestType.Job_Based)
            {
                masterRoot = repoPath;
                repoPath = FindFolder();
            }

            AddFiles();
            fileDiffs = new bool[files.Length + 1];
            repoMatch = new string[files.Length + 1];
        }

        /// <summary>
        /// Used to run the change request in accordance with the parameters that were entered in the constructor
        /// </summary>
        public void Submit()
        {
            Git git = new Git(repoPath);
            bool change = false;
            Hide(repoPath, true);
            string[] repoFiles = Directory.GetFiles(repoPath, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                string subfile = files[i];
                foreach (string repofile in repoFiles)
                {
                    if (Path.GetFileName(subfile) == Path.GetFileName(repofile))
                    {
                        repoMatch[i] = repofile;
                        fileDiffs[i] = Diff(subfile, repofile, repoPath);
                        if (fileDiffs[i]) change = true;
                    }
                }
            }
            if (change)
            {
                string branch = "Change_" + DateTime.Now.ToString("MM.dd.yyyy_hh.mm.ss");
                git.Branch(branch);
                git.Checkout(branch);
                for (int j = 0; j < files.Length; j++)
                {
                    if (fileDiffs[j])
                    {
                        FileChange(files[j], repoMatch[j]);
                        git.Stage();
                        git.Commit(System.IO.Path.GetFileName(files[j]));
                    }
                }
            }
            Hide(repoPath, false);
            Hide(repoPath + @"\.git", true);
            git.Checkout("master");
        }

        //Add in the files from Submission info into the files array
        private void AddFiles()
        {
            if (File.Exists(subPath))
            {
                string[] _files = { subPath };
                files = _files;
            }
            else if (Directory.Exists(subPath))
            {
                files = Directory.GetFiles(subPath);
            }
        }


        //Private class that is used to actually carry out the changing, i.e. copy and pasting of the submitted file(s) to the newly created branch
        private void FileChange(string _subPath, string _repoFile)
        {
            if (File.Exists(_subPath) == File.Exists(_repoFile)) {
                File.Copy(_subPath, _repoFile, true);
            }
        }

        //Find the jobs folder for the job# given
        private string FindFolder()
        {
            string dir = null;
            string[] paths = System.IO.Directory.GetDirectories(masterRoot, "*", System.IO.SearchOption.AllDirectories);
            foreach (string path in paths)
            {
                string dirChck = new System.IO.DirectoryInfo(path).Name;
                if (dirChck == jobNumb)
                {
                    dir = path;
                }
            }
            return dir;
        }

        //Returns true is the files differ
        //  Is set to true by default so the highest priority is default
        private bool Diff(string _subFilePath, string _repoFilePath, string _repoPath)
        {
            List<string> branches = new List<string>();
            bool differ = true;

            using (LibGit2Sharp.Repository _repo = new LibGit2Sharp.Repository(_repoPath))
            {
                foreach (LibGit2Sharp.Branch b in _repo.Branches)
                {
                    branches.Add(b.ToString());
                }
                int i = 0;
                while (differ == true && i < branches.Count)
                {
                    LibGit2Sharp.Commands.Checkout(_repo, branches[i]);
                    int byte1;
                    int byte2;
                    try
                    {
                        var fs1 = new FileStream(_subFilePath, FileMode.Open);
                        var fs2 = new FileStream(_repoFilePath, FileMode.Open);
                        do
                        {
                            byte1 = fs1.ReadByte();
                            byte2 = fs2.ReadByte();
                        } while ((byte1 == byte2) && (byte1 != -1));
                        fs1.Close();
                        fs2.Close();
                        differ = ((byte1 - byte2) != 0);
                    }
                    catch (Exception e)
                    {
                        differ = true;
                    }
                    i++;
                }
            }
            return differ;
        }
        private void DeleteFile(string _path)
        {
            System.IO.File.Delete(_path + @"\" + _path);
        }

        /// <summary>
        /// Hides or unhides a given file, or folder
        /// </summary>
        /// <param name="_path">The pathway to the file, or folder</param>
        /// <param name="_hide"><c>Boolean</c>:Set to <c>true</c> to hide, and set to <c>false</c> to unhide.<para>Set to <c>true</c> by default</para></param>
        public void Hide(string _path, bool _hide = true)
        {
            if (_hide)
            {
                try
                {
                    if (Directory.Exists(_path))
                    {
                        System.IO.File.SetAttributes(_path, System.IO.FileAttributes.Hidden);
                    }
                }
                catch (Exception e)
                { }
            }
            else
            {
                try
                {
                    System.IO.File.SetAttributes(_path, System.IO.FileAttributes.Normal);
                }
                catch (Exception e)
                { }
            }
        }
    }

    /// <summary>
    /// Used to send emails from within Acutec's PA and SC facilities
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Indicates the source email address that will send the email notification
        /// </summary>
        public enum Smtp
        {
            /// <summary>
            /// Acutec's noreply email set up for PA, administrator access will need to be used inorder to use this I believe
            /// </summary>
            AcutecPA,
            /// <summary>
            /// Acutec's llcit@acutecind.com address set up for use in Acutec's South Carolina location
            /// </summary>
            AcutecSC,
            /// <summary>
            /// Setup for testing only
            /// </summary>
            //Test,
        }

        Smtp SmtpServer = Smtp.AcutecPA;

        private string acutecpa_smtp = "mail.acutec.local";
        private string acutecsc_smtp = "smtp.office365.com";
        private string testing_smtp = " smtp.gmail.com";

        private string acutecpa_add = "noreply@acutecprecision.com";
        private string acutecsc_add = "llcit@acutecind.com";
        //private string testing_add = "testemail@notarealemail.com";

        private System.Net.NetworkCredential acutecsc_creds = new System.Net.NetworkCredential("llcit@acutecind.com", "fRD82W64");
        private System.Net.NetworkCredential acutecpa_creds = new System.Net.NetworkCredential("noreply@acutecprecision.com", "");
        //private System.Net.NetworkCredential testing_creds = new System.Net.NetworkCredential("username","password");

        int SmtpPort = 25;
        string SmtpServerAddress = "mail.acutec.local";
        string[] Recievers = null;
        string MailingListPath = null;
        System.Net.NetworkCredential Credentials = new System.Net.NetworkCredential("noreply@acutecprecision.com", "");
        string Body = "This email has been sent as part of the automated AcuGit System.\n" + "Message sent: " + DateTime.Now.ToString();
        string Subject = "Automated Email Response as Part of AcuGit System";

        /// <summary>
        /// Simplest constructor of the Email Class
        /// </summary>
        /// <param name="_SmtpServer">Indicate whether you would like to use Acutec's PA, or SC email for notification</param>
        /// <param name="_Recievers"><c>string[]</c>:An array of the email addresses you would like to send the notification to</param>
        public Email(Smtp _SmtpServer, string[] _Recievers) {
            SmtpServer = _SmtpServer;
            Recievers = _Recievers;
        }
        /// <summary>
        /// Constructor where one can set up a custom subject and body
        /// </summary>
        /// <param name="_SmtpServer">Indicate whether you would like to use Acutec's PA, or SC email for notification</param>
        /// <param name="_Recievers"><c>string[]</c>:An array of the email addresses you would like to send the notification to</param>
        /// <param name="_Subject">The subject of the email</param>
        /// <param name="_Body">The body of the email</param>
        public Email(Smtp _SmtpServer, string[] _Recievers, string _Subject, string _Body)
        {
            SmtpServer = _SmtpServer;
            Subject = _Subject;
            Body = _Body;
            Recievers = _Recievers;
        }
        /// <summary>
        /// Simplest constructor that takes the path to a .csv file for the mailing list
        /// </summary>
        /// <param name="_SmtpServer">Indicate whether you would like to use Acutec's PA, or SC email for notification</param>
        /// <param name="_MailingList">The path to a .csv file holds all of the addresses that you would like to send the notification</param>
        public Email(Smtp _SmtpServer, string _MailingList)
        {
            SmtpServer = _SmtpServer;
            MailingListPath = _MailingList;
            string List = System.IO.File.ReadAllText(_MailingList);
            Recievers = List.Split(',');
        }
        /// <summary>
        /// Allows the user to add custom Subject and body, as well as, using a .csv file as a mailing list
        /// </summary>
        /// <param name="_SmtpServer">Indicate whether you would like to use Acutec's PA, or SC email for notification</param>
        /// <param name="_MailingList">The path to a .csv file holds all of the addresses that you would like to send the notification</param>
        /// <param name="_Subject">The subject of the email</param>
        /// <param name="_Body">The body of the email</param>
        public Email(Smtp _SmtpServer, string _MailingList, string _Subject, string _Body)
        {
            SmtpServer = _SmtpServer;
            Subject = _Subject;
            Body = _Body;
            MailingListPath = _MailingList;
            string List = System.IO.File.ReadAllText(_MailingList);
            Recievers = List.Split(',');
        }
        /// <summary>
        /// Sends out the email you have constructed
        /// </summary>
        public void Send() {
            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            MailAddress From = null;
            if (SmtpServer == Smtp.AcutecSC)
            {
                SmtpServerAddress = acutecsc_smtp;
                SmtpPort = 587;
                client.Credentials = acutecsc_creds;
                From = new MailAddress(acutecsc_add, "Acutec Task Services");
            }
            else if (SmtpServer == Smtp.AcutecPA)
            {
                SmtpServerAddress = acutecpa_smtp;
                SmtpPort = 25;
                client.Credentials = acutecpa_creds;
                From = new MailAddress(acutecpa_add, "Acutec Task Services");
            }
            /*else if (SmtpServer == Smtp.Test) {
                SmtpServerAddress = testing_smtp;
                SmtpPort = 25;
                client.Credentials = testing_creds;
                From = testing_add;
            }*/
            client.EnableSsl = true;
            client.Port = SmtpPort;
            client.Host = SmtpServerAddress;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            string date = DateTime.Now.ToString();
            for (int i = 0; i < Recievers.Length; i++) {
                MailAddress To = new MailAddress(Recievers[i]);
                MailMessage msg = new MailMessage(From, To);
                msg.Subject = Subject;
                msg.Body = Body;
                try
                {
                    client.Send(msg);
                    msg.Dispose();
                }
                catch (Exception e) {
                    //using (StreamWriter stream = File.CreateText(@"Path To File"+DateTime.Now.ToString("___MM_dd_yyyy_hh_mm_ss"))) {
                    //   stream.Write(e);
                    //}
                }

            }
        }
    }
    /// <summary>
    /// Class built for user friendly interaction between C# and Git
    /// </summary>
    public class Git
    {
        //This class will contain all git library commands
        private string repoPath = null;
        /// <summary>
        /// Indicates the sort of action that will be taken with the Git.Reset command
        /// </summary>
        public enum ResetMode
        {
            /// <summary>
            /// A Soft reset will simply reverse the working directory to the indicated commit leaving any staged or changed files alone 
            /// </summary>
            Soft,
            /// <summary>
            /// A Mixed reset (Note: Set to the default of Git.Reset()) will reverse the working directory to the indicated commit while also removing all files from staging 
            /// </summary>
            Mixed,
            /// <summary>
            /// A Hard reset will not only reverse the working directory to the indicated commit it will also undo any possible changes to any files within the repository
            /// </summary>
            Hard
        }
        LibGit2Sharp.Signature sign = new LibGit2Sharp.Signature(new LibGit2Sharp.Identity("(Automated)", "(--)"), DateTime.Now);
        /// <summary>
        /// The list of commit info for this repository
        /// </summary>
        public List<CommitInfo> Commitlist = new List<CommitInfo>();

        /// <summary>
        /// Constructor of the Git class, takes 1 parameter
        /// </summary>
        /// <param name="_repoPath">The pathway to repository you would like to access, or create</param>
        public Git(string _repoPath)
        {
            repoPath = _repoPath;
        }
        /// <summary>
        /// Stage files to be commited to the repository
        /// </summary>
        /// <param name="_fileName">The name of the file that you would like to stage<para>Set to stage all files by default</para></param>
        public void Stage(string _fileName = "*")
        {
            using (var repo = new LibGit2Sharp.Repository(repoPath))
            {
                try
                {
                    LibGit2Sharp.Commands.Stage(repo, _fileName);
                }
                catch (Exception e)
                { }
            }
        }
        /// <summary>
        /// Unstages files that were to be commited, or can be used to both unstage files
        /// Running this command with the default values will act as an "Unstage -all" command
        /// </summary>
        /// <param name="_ResetMode">Indicates the <c>ResetMode</c> that the user would like to use <para><see cref="ResetMode"/></para></param>
        /// <param name="_stepBack">The number of commits the user would like to step back<para>    Set to 0 by default, indicates a reset back to the top commit of that branch</para></param>
        public void Reset(ResetMode _ResetMode = ResetMode.Mixed, int _stepBack = 0)
        {

            //!!!!!Untested!!!!!!

            LibGit2Sharp.ResetMode _libGitResetMode = LibGit2Sharp.ResetMode.Mixed;
            if (_ResetMode == ResetMode.Hard)
            {
                _libGitResetMode = LibGit2Sharp.ResetMode.Hard;
            }
            else if (_ResetMode == ResetMode.Soft)
            {
                _libGitResetMode = LibGit2Sharp.ResetMode.Soft;
            }
            using (var repo = new LibGit2Sharp.Repository(repoPath))
            {
                LibGit2Sharp.Commit comm = repo.Head.Tip;
                if (_stepBack != 0)
                {
                    comm = repo.Head.Commits.ElementAt(_stepBack);
                }
                repo.Reset(_libGitResetMode, comm);
            }
        }
        /// <summary>
        /// Commit the files/changes that have been staged
        /// </summary>
        /// <param name="_message">The commit message, or information</param>
        /// <param name="_username">The username, or identity of the party that is submitting the commit<para>Set to <c>"(Automated)"</c> by default</para></param>
        /// <param name="_email_Address">The email address of the user that that is making the commit<para>Set to <c>"--"</c> by default</para></param>
        public void Commit(string _message, string _username = "(Automated)", string _email_Address = "--")
        {
            if (_username == null || _username == "")
            {
                _username = "(Automated)";
            }
            LibGit2Sharp.Identity id = new LibGit2Sharp.Identity(_username, _email_Address);
            LibGit2Sharp.Signature sign = new LibGit2Sharp.Signature(id, DateTime.Now);
            using (var repo = new LibGit2Sharp.Repository(repoPath))
            {
                try
                {
                    repo.Commit(_message, sign, sign);
                }
                catch (Exception e)
                {
                }
            }
        }
        /// <summary>
        /// Quick commit (Note: Use this method only for very simple/minor commits)
        /// </summary>
        public void Commit()
        {
            string _message = "(Quick Commit)" + DateTime.Now.ToString("MM/dd/yy(hh:mm:ss)");
            using (var repo = new LibGit2Sharp.Repository(repoPath))
            {
                try
                {
                    repo.Commit(_message, sign, sign);
                }
                catch (Exception e)
                {
                }
            }
        }
        /// <summary>
        /// Creates a repository in path of the Git object
        /// </summary>
        public void Init()
        {
            try
            {
                LibGit2Sharp.Repository.Init(repoPath);
            }
            catch (Exception e)
            { }
        }
        /// <summary>
        /// Creates a new branch in the repository of the Git objects repository <para>***Important*** There needs to be a repository in this directory in order for branching to be possible</para>
        /// </summary>
        /// <param name="branchName">The name that you would like to name the branch</param>
        public void Branch(string branchName)
        {
            using (var repo = new LibGit2Sharp.Repository(repoPath))
            {
                try
                {
                    repo.CreateBranch(branchName);
                }
                catch (Exception e)
                {

                }
            }
        }
        /// <summary>
        /// Checks-out a specific branch within this Git objects repository<para>***Important*** commits, additions, and stages, will only be added in the branch that is checked out</para>
        /// <para>The Master repository of repositories made with this tool will always be named "master"</para>
        /// </summary>
        /// <param name="branchName">The name of the branch that you would like to be checked out</param>
        public void Checkout(string branchName)
        {
            try
            {
                using (var repo = new LibGit2Sharp.Repository(repoPath))
                {
                    var branch = repo.Branches[branchName];
                    LibGit2Sharp.Commands.Checkout(repo, branch);
                }
            }
            catch (Exception e)
            {

            }
        }
        /// <summary>
        /// Set or reset the path of the Git object, i.e. change the working directory of this object
        /// </summary>
        /// <param name="path"></param>
        public void SetPath(string path)
        {
            repoPath = path;
        }
        /// <summary>
        /// Return the path of this objects working repository
        /// </summary>
        /// <returns></returns>
        public string GetPath()
        {
            return repoPath;
        }
        /// <summary>
        /// This method will create the initial .gitignore file and add any ignore parameters as well if there are any
        /// </summary>
        public void CreateIgnore()
        {
            //Checks first if the .gitignore file does not exist already
            if (!System.IO.File.Exists(repoPath + @"\.gitignore"))
            {
                //Create  .gitignore file
                using (FileStream stream = new FileStream(repoPath + @"\.gitignore", FileMode.Create)) { };
            }
            //if the ignore list is null then the program will end
            //else it will call AddIgnore for every string in ignoreList
        }
        /// <summary>
        /// Adds new ignoreable arguments to the .gitignore file in this repository, or Creates the .gitignore file if it does not exist already
        /// </summary>
        /// <param name="ignoreables"><c>string[]</c>: Array of arguments/parameters the user would like to add to the .gitignore file</param>
        public void AddIgnore(string[] ignoreables)
        {

            //!!!!!Untested!!!!!!
            string ign = repoPath + @"\.gitignore";
            //AddIgnore can be used to add lines to an already existing .gitignore file
            //If a .gitignore file does not already exist it will then call CreateIgnore() and pass its ignoreable(s)
            if (System.IO.File.Exists(ign))
            {
                using (FileStream fs = new FileStream(ign, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        foreach (string ignore in ignoreables)
                        {
                            sw.WriteLine(ignore);
                        }
                    }
                }
            }
            else
            {
                //!!!!!Untested!!!!!!
                CreateIgnore();
                AddIgnore(ignoreables);
            }
        }
        /// <summary>
        /// Adds new ignoreable argument to the .gitignore file in this repository, or Creates the .gitignore file if it does not exist already
        /// </summary>
        /// <param name="ignoreable"><c>string</c>: the argument you would like to add to the .gitignore file</param>
        public void AddIgnore(string ignoreable)
        {
            string[] ignoreables = new string[] { ignoreable };
            if (System.IO.File.Exists(repoPath + @"\.gitignore"))
            {
                using (FileStream fs = new FileStream((repoPath + @"\.gitignore"), FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(ignoreable);
                    }
                }
            }
            else
            {
                CreateIgnore();
                AddIgnore(ignoreables);

            }
        }
        /// <summary>
        /// Erases all existing parameters/arguments in the .gitignore file
        /// </summary>
        public void ResetIgnore()
        {

            if (System.IO.File.Exists(repoPath + @"\.gitignore"))
            {
                System.IO.File.Delete(repoPath + @"\.gitignore");
                CreateIgnore();
            }
            else
            {
                CreateIgnore();
            }
        }
        /// <summary>
        /// A method for returning the arguments/lines in the .gitignore file if it exists
        /// </summary>
        /// <returns>returns a list delimited by commas of all the extension types and arguments that the repository will ignore</returns>
        public string GetIgnore()
        {

            //!!!!!Untested!!!!!!

            string ignoreList = null;
            if (System.IO.File.Exists(repoPath + @"\.gitignore"))
            {
                string[] ignoreArray = System.IO.File.ReadAllLines((repoPath + @"\.gitignore"));
                foreach (string s in ignoreArray)
                {
                    ignoreList += s + " ,";
                }
            }
            return ignoreList;
        }
        /// <summary>
        /// Parses the .gitignored file to check if the given argument is being ignored
        /// </summary>
        /// <param name="arg">The ignore argument that is being checked for</param>
        /// <returns><c>bool</c>:<c>true</c> if the argument is being ignored already <c>false</c> if the argument is not being ignored, or .gitignore does not exist</returns>
        public bool Ignored(string arg)
        {

            //!!!!!Untested!!!!!!

            bool ignored = false;
            if (!System.IO.File.Exists(repoPath + @"\.gitignore")) return false;
            else
            {
                string[] ignoreArray = System.IO.File.ReadAllLines((repoPath + @"\.gitignore"));
                int i = 0;
                do
                {
                    if (arg == ignoreArray[i])
                    {
                        ignored = true;
                    }
                    i++;
                }
                while (!ignored && i < ignoreArray.Length);
            }
            return ignored;
        }
        /// <summary>
        /// Deletes the given file, as well as, commiting the removal to git
        /// </summary>
        /// <param name="_filename"> The filename or path to the file that you would like to remove</param>
        public void Remove(string _filename)
        {

            //!!!!!Untested!!!!!!

            //Deletes a file in the repository, stage the deletion, and commit
            string filename = null;

            //Find the full path of the file entered
            if (System.IO.File.Exists(_filename))
            {
                filename = _filename;
            }
            else
            {
                string[] _repoFiles = Directory.GetFiles(repoPath, "*.*", SearchOption.AllDirectories);
                foreach (string file in _repoFiles)
                {
                    if (Path.GetFileName(_filename) == Path.GetFileName(file))
                    {
                        filename = file;
                    }
                }
            }
            //if the filename is not null then delete the file, stage the change, and commit
            if (filename != null)
            {
                System.IO.File.Delete(filename);
                Stage();
                Commit();
            }
        }
        /// <summary>
        /// Removes the given file from being tracked by Git
        /// </summary>
        /// <param name="_filename"> The file or the path to the file you would like to stop tracking</param>
        public void Untrack(string _filename)
        {

            //!!!!!Untested!!!!!!

            //Will move a file out of the working directory (out of the repository), 
            //stage and commit the change, 
            //then add the file to .gitignore, 
            //then stage and commit .gitignore, 
            //then move the file back into the repository
            string filename = null;

            //Find the full path of the file entered
            if (System.IO.File.Exists(_filename))
            {
                filename = _filename;
            }
            else
            {
                string[] _repoFiles = Directory.GetFiles(repoPath, "*.*", SearchOption.AllDirectories);
                foreach (string file in _repoFiles)
                {
                    if (Path.GetFileName(_filename) == Path.GetFileName(file))
                    {
                        filename = file;
                    }
                }
            }
            //if the filename is not null then delete the file, stage the change, and commit
            if (filename != null)
            {
                System.IO.File.Copy(filename, System.IO.Path.GetDirectoryName(filename) + @"\.UnTrackTemp", true);
                System.IO.File.SetAttributes(System.IO.Path.GetDirectoryName(filename) + @"\.UnTrackTemp", FileAttributes.Hidden);
                System.IO.File.Delete(filename);
                if (!Ignored(".UnTrackTemp"))
                {
                    AddIgnore(".UnTrackTemp");
                }
                if (!Ignored(System.IO.Path.GetFileName(filename)))
                {
                    AddIgnore(System.IO.Path.GetFileName(filename));
                }
                Stage();
                Commit();
                System.IO.File.Copy(System.IO.Path.GetDirectoryName(filename) + @"\.UnTrackTemp", filename, true);
                System.IO.File.Delete(System.IO.Path.GetDirectoryName(filename) + @"\.UnTrackTemp");
            }
        }
        /// <summary>
        /// This command will build, or rebuild, the list of commits for this repository
        /// </summary>
        public void CommitList()
        {
            Commitlist.Clear();
            using (var repo = new LibGit2Sharp.Repository(repoPath))
            {
                foreach (LibGit2Sharp.Branch branch in repo.Branches)
                {
                    LibGit2Sharp.Commands.Checkout(repo, branch);
                    foreach (LibGit2Sharp.Commit comm in repo.Commits)
                    {
                        Commitlist.Add(new CommitInfo(branch.FriendlyName, comm));
                    }
                }
                LibGit2Sharp.Commands.Checkout(repo, "master");
            }
        }
        /// <summary>
        /// Cherry Pick a commit based off of the CommitInfo class, this would make it easier to work with the CommitList method and the <c>List Commits</c> object
        /// </summary>
        /// <param name="info">The commit info that you wish to pass <see cref="CommitInfo"/></param>
        public void CommitPick(CommitInfo info)
        {
            LibGit2Sharp.Branch Working = null;
            using (var repo = new LibGit2Sharp.Repository(repoPath))
            {
                LibGit2Sharp.Commit commit = null;
                foreach (LibGit2Sharp.Branch bran in repo.Branches)
                {
                    if (info.BranchName == bran.FriendlyName)
                    {
                        Working = bran;
                        LibGit2Sharp.Commands.Checkout(repo, bran);
                    }
                    foreach (LibGit2Sharp.Commit comm in repo.Commits)
                    {
                        if (info.CommitShortMessage == comm.MessageShort) commit = comm;
                    }
                }
                LibGit2Sharp.CherryPickResult result = null;
                LibGit2Sharp.Commands.Checkout(repo, "master");
                try
                {
                    result = repo.CherryPick(commit, sign);
                }
                catch (Exception e)
                {
                    HardPick(commit, Working);
                }
                try
                {
                    if (result.Commit == null)
                    {
                        HardPick(commit, Working);
                    }
                }
                catch (Exception e)
                {
                }
            }
        }
        private void HardPick(LibGit2Sharp.Commit commit, LibGit2Sharp.Branch Working) {
            //first find the last commit in the master before this branch differs
            Reset(ResetMode.Hard);
            using (var repo = new LibGit2Sharp.Repository(repoPath))
            {
                LibGit2Sharp.Branch master = null;
                foreach (LibGit2Sharp.Branch b in repo.Branches)
                {
                    if (b.FriendlyName == "master") {
                        master = b;
                    }
                }
                LibGit2Sharp.Commit orig_Tip = repo.Head.Tip;
                Stack<LibGit2Sharp.Commit> WorkingCommits = new Stack<LibGit2Sharp.Commit>();
                Stack<LibGit2Sharp.Commit> MasterCommits = new Stack<LibGit2Sharp.Commit>();
                if (master != null)
                {
                    MasterCommits = GetCommits(master);
                }
                if (Working != null)
                {
                    WorkingCommits = GetCommits(Working);
                }
                LibGit2Sharp.Commit last = null;
                int max = WorkingCommits.Count();
                for(int i = 0; i < max;i++) {
                    //Find the last shared commit
                    if (MasterCommits.Peek() == WorkingCommits.Peek())
                    {
                        last = MasterCommits.Pop();
                        WorkingCommits.Pop();
                    }
                    else {
                        break;
                    }
                }
                if (last != null) {
                    //Create a new branch and hard reset it to the "last commit"
                    LibGit2Sharp.Branch temp = repo.CreateBranch("Temp");
                        //Checkout the new branch
                    LibGit2Sharp.Commands.Checkout(repo, "Temp");
                        //Hard reset the new branch to "Last"
                    repo.Reset(LibGit2Sharp.ResetMode.Hard, last);
                    //Cherry Pick the original commit into the new branch
                    LibGit2Sharp.CherryPickResult result = repo.CherryPick(commit, sign);
                    //Cherry-Pick all the commits following "last" from master onto Temp
                        //Need to switch order as well to go to top down through Master Commits, that way latest changes are kept when reverting conflicted commits
                    Stack<LibGit2Sharp.Commit> TopDownMasterCommits = new Stack<LibGit2Sharp.Commit>();
                    foreach (LibGit2Sharp.Commit C in MasterCommits) {
                        TopDownMasterCommits.Push(C);
                    }
                    foreach (LibGit2Sharp.Commit C in TopDownMasterCommits) {
                        LibGit2Sharp.CherryPickResult r = repo.CherryPick(C, sign);
                        try
                        {
                            if (r.Commit == null)
                            {
                                //Clean the dirtied index
                                Reset(ResetMode.Hard);
                                //Checkout Master
                                LibGit2Sharp.Commands.Checkout(repo, "master");
                                    //Revert the conflicted commit on master
                                bool rStat = Revert(C);
                                //If the revert fails then clean index
                                if (!rStat) {
                                    Reset(ResetMode.Hard);
                                }
                                //Checkout Temp and Continue with picking
                                LibGit2Sharp.Commands.Checkout(repo, "Temp");
                            }
                        }
                        catch (Exception e) {
                            continue;
                        }
                    }
                    //Soft reset the new branch to top of the master
                        //Get the tip commit
                    LibGit2Sharp.Commands.Checkout(repo, "master");
                    LibGit2Sharp.Commit tip = repo.Head.Tip;
                        //Then reset to it
                    LibGit2Sharp.Commands.Checkout(repo, "Temp");
                    repo.Reset(LibGit2Sharp.ResetMode.Soft, tip);
                    //Checkout master, Stage and commit all
                    LibGit2Sharp.Commands.Checkout(repo, "master");
                    Stage();
                    Commit(commit.Message);
                    //Delete new branch
                    repo.Branches.Remove("Temp");
                    repo.Reset(LibGit2Sharp.ResetMode.Soft, orig_Tip);
                    Stage();
                    Commit("Hard Picked: " + commit.MessageShort);
                }
            }
        }
        private Stack<LibGit2Sharp.Commit> GetCommits(LibGit2Sharp.Branch B) {
            Stack<LibGit2Sharp.Commit> CStack = new Stack<LibGit2Sharp.Commit>();
            foreach (LibGit2Sharp.Commit C in B.Commits)
            {
                CStack.Push(C);
            }
            return CStack;
        }
        /// <summary>
        /// Checks if the path that you are accessing is a Git Repository
        /// </summary>
        /// <returns><c>bool</c></returns>
        public bool Exists()
        {
            return LibGit2Sharp.Repository.IsValid(repoPath);
        }
        /// <summary>
        /// This method is setup to revert a given commit, then return a boolean representing whether or not it was possible
        /// </summary>
        /// <param name="commit">The Libgit2sharp.Commit that you wish to revert(delete)</param>
        /// <returns><c>bool</c>: Returns false if the revert fails, true if the revert succeeds</returns>
        public bool Revert(LibGit2Sharp.Commit commit) {
            bool val = false;
            using (var repo = new Repository(repoPath))
            {
                LibGit2Sharp.RevertResult result = repo.Revert(commit,sign);
                if (result.Status == RevertStatus.Conflicts)
                {
                    val = false;
                }
                else {
                    val = true;
                }
                return val;
            }
        }
        /// <summary>
        /// This method is setup to revert a given commit based off of the <c>CommitInfo</c> class
        ///     <see cref="CommitInfo"/>
        /// </summary>
        /// <param name="info"><c>CommitInfo</c>: The CommitInfo object pertaining to the commit you would like to revert</param>
        /// <returns></returns>
        public bool Revert(CommitInfo info)
        {
            bool val = false;
            using (var repo = new Repository(repoPath))
            {
                LibGit2Sharp.RevertResult result = repo.Revert(info.commit, sign);
                if (result.Status == RevertStatus.Conflicts)
                {
                    val = false;
                }
                else
                {
                    val = true;
                }
                return val;
            }
        }
        /// <summary>
        /// Deletes the given branch
        /// </summary>
        /// <param name="branchName"><c>string</c>: The user friendly name of the branch that you wish to delete</param>
        public void DeleteBranch(string branchName) {
            using (var repo = new LibGit2Sharp.Repository(repoPath)) {
                try
                {
                    repo.Branches.Remove(branchName);
                }
                catch (Exception e) { }
            }
        }
        /// <summary>
        /// Deletes the given branch, based off the LibGit2Sharp.Branch class
        /// </summary>
        /// <param name="branch"><c>LibGit2Sharp.Branch</c>: The branch that you wish to delete</param>
        public void DeleteBranch(LibGit2Sharp.Branch branch)
        {
            using (var repo = new LibGit2Sharp.Repository(repoPath))
            {
                try
                {
                    repo.Branches.Remove(branch);
                }
                catch (Exception e) { }
            }
        }
        /// <summary>
        /// Returns an array of the changes between the master version of a file, and the version in a given branch
        /// </summary>
        /// <param name="fileName">The file that you wish to diff</param>
        /// <param name="branchName">The name of the branch you wish to compare against the master</param>
        /// <returns>An array of diff line statements including the line that the change has occured in</returns>
        public string[] Diff(string fileName, string branchName) {
            string[] files = Directory.GetFiles(repoPath);
            string filePath = "";
            foreach (string file in files) {
                if (Path.GetFileName(file) == fileName) {
                    filePath = file;
                }
            }
            string[] master_Lines;
            if (filePath != "")
            {
              master_Lines = File.ReadAllLines(filePath);
            }
            else {
                return null;
            }
            using (var repo = new LibGit2Sharp.Repository(repoPath)) {
                LibGit2Sharp.Commands.Checkout(repo, branchName);
                string[] branch_Lines = File.ReadAllLines(filePath);
                if (branch_Lines != null && master_Lines != null)
                {
                    int max = branch_Lines.Length;
                    if (master_Lines.Length > max) max = master_Lines.Length;
                    LibGit2Sharp.Commands.Checkout(repo, "master");
                    return lineDiffer(master_Lines, branch_Lines);
                }
                else {
                    LibGit2Sharp.Commands.Checkout(repo, "master");
                    return null;
                }
            }
        }
        private string[] lineDiffer(string[] lines_1, string[] lines_2) {
            List<string> response = new List<string>();
            int Count = lines_1.Length;
            if (lines_2.Length > Count) Count = lines_2.Length;
            int Max = 0;
            foreach (string s in lines_2)
            {
                if (s.Length > Max) Max = s.Length;
            }
            Max += 3;
            for (int i = 0; i < Count; i++)
            {
                string type = "===";
                try
                {
                    if (lines_1[i] != lines_2[i])
                    {
                        type = "Diff";
                    }
                }
                catch (Exception e)
                {
                    type = "????";
                    try
                    {
                        type = "-End";
                    }
                    catch (Exception e2)
                    {
                        type = "+End";
                    }
                }
                string s = type;
                switch (type) {
                    case "Diff":
                        s = String.Format("Line {2} >>> Diff New:{0} Old:{1}", lines_2[i].PadRight(Max), lines_1[i].PadRight(Max), (i + 1).ToString().PadRight(4));
                        response.Add(s);
                        break;
                    case "+End":
                        s = String.Format("Line {2} >>> {1} New:{0}", lines_2[i].PadRight(Max), "++++".PadRight(3), (i + 1).ToString().PadRight(4));
                        response.Add(s);
                        break;
                    case "-End":
                        s = String.Format("Line {2} >>> {0} Old:{1}", "----".PadRight(3), lines_1[i].PadRight(Max), (i + 1).ToString().PadRight(4));
                        response.Add(s);
                        break;
                }
                
            }
            return response.ToArray();
        }
    }
    /// <summary>
    /// Stores non-LibGit2 library info for referencing a Commit
    /// </summary>
    public class CommitInfo{
        /// <summary>
        /// The Branch the commit is in
        /// </summary>
        public string BranchName;
        /// <summary>
        /// The Shortened message of the commit (typically the first line)
        /// </summary>
        public string CommitShortMessage;
        /// <summary>
        /// The Commit ID of the commit
        /// </summary>
        public string ID;
        /// <summary>
        /// The <c>LibGit2Sharp.Commit</c> that this commit points to
        /// </summary>
        public LibGit2Sharp.Commit commit = null;
        /// <summary>
        /// The <c>LibGit2Sharp.Branch</c> that this commit is in
        /// </summary>
        public LibGit2Sharp.Branch branch = null;
        /// <summary>
        /// Constructor of the CommitInfo class
        ///     This Constructor will leave the <c>LibGit2Sharp</c> Branch object set to null
        /// </summary>
        /// <param name="branchName">The Branch the commit is in</param>
        /// <param name="_commit">The LibGit2 Commit object that this info is based around</param>
        public CommitInfo(string branchName, LibGit2Sharp.Commit _commit) {
            BranchName = branchName;
            CommitShortMessage = commit.MessageShort;
            ID = commit.Id.ToString();
            commit = _commit;
        }
        /// <summary>
        /// Intitialize CommitInfo based off of <c>LibGit2Sharp</c> classes
        ///     Most solid constructor
        /// </summary>
        /// <param name="_branch"><c>LibGit2Sharp.Branch</c>: The branch that this commit resides in</param>
        /// <param name="_commit"><c>LibGit2Sharp.Commit</c>: The commit object that this info pertains to</param>
        public CommitInfo(LibGit2Sharp.Branch _branch, LibGit2Sharp.Commit _commit)
        {
            BranchName = _branch.FriendlyName;
            CommitShortMessage = commit.MessageShort;
            ID = commit.Id.ToString();
            commit = _commit;
            branch = _branch;
        }
    }
}
