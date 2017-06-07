using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AcuGitLibrary
{
    public class ChangeRequest
    {
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
        /// <summary>
        /// Indicates whether the user is submitting an entire folder or simply singular file
        /// </summary>
        public enum SubmissionType
        {
            /// <summary>
            /// The user is submitting a folder, and there by all files that are within
            /// </summary>
            Multi,
            /// <summary>
            /// The user is only submitting one file, to which they will provide the path
            /// </summary>
            Single
        }

        public SubmissionType subType = SubmissionType.Single;
        public RequestType reqType = RequestType.Precise;

        string subPath = null;
        string repoPath = null;
        string masterRoot = null;

        string[] files = null;

        private string branchName = null;
        private string commitMsg = null;
        private bool progChange = false;
        bool deleteSub = false;

        private string submitterName = "(Automated)";

        string jobNumb;
        string opNumb;
        string partNumb;
        string DWGRev;
        string machineNumb;
        string machineProgNumb;
        string releaseNumb;
        string toolingInfo;
        bool setupChanges;

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
        /// <param name="_reqType"><c>RequestType</c>: set to <c>RequestType.Precise</c> by default</param>
        /// <param name="_subType"><c>SubmissionType</c>: set to <c>SubmissionType.Single</c> by default</param>
        /// <param name="_deleteSub"></param>
        public ChangeRequest(string _subPath,string _repoPath, RequestType _reqType = RequestType.Precise, SubmissionType _subType = SubmissionType.Single, bool _deleteSub = false, string _jobNumb = null)
        {
            reqType = _reqType;
            subType = _subType;
            subPath = _subPath;
            repoPath = _repoPath;
            deleteSub = _deleteSub;
            jobNumb = _jobNumb;

            if (reqType == RequestType.Job_Based) {
                masterRoot = repoPath;
                repoPath = FindFolder();
            }

            AddFiles();
        }

        /// <summary>
        /// Used to run the change request in accordance with the parameters that were entered in the constructor
        /// </summary>
        public void Submit(){
            //Hide the master repo
            foreach (string _path in files) {
                MakeChange(_path);
            }
            //unhide the master
            //rehide the .git folder
            //Checkout master
        }

        //Add in the files from Submission info into the files array
        private void AddFiles() {
            if (File.Exists(subPath)) {
                string[] _files = { subPath };
                files = _files;
            } else if (Directory.Exists(subPath)) {
                files = Directory.GetFiles(subPath);
            }
        }

        //Private class that will be called in submit to make the individual file changes
        private void MakeChange(string _path) {
            
            //Make new branch
            //Make file change
            //Stage
            //Commit
        }

        //Private class that is used to actually carry out the changing, i.e. copy and pasting of the submitted file(s) to the newly created branch
        private bool FileChange()
        {
            bool changed = false;
            return changed;
        }

        //Find the jobs folder for the job# given
        private string FindFolder() {
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
        private bool Diff(string _subFilePath, string _repoFilePath)
        {
            bool differ = true;
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
            return differ;
        }
        private void DeleteFile(string _path)
        {
            System.IO.File.Delete(_path + @"\" + fileName);
        }
        /// <summary>
        /// Allows the user to set the name they would like the branch to be
        /// </summary>
        /// <param name="_branchName">The name that the branch of submission will be given in git</param>
        public void SetBranchName(string _branchName)
        {
            branchName = _branchName;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>the name of the branch that is currently set in the change requester</returns>
        public string GetBranchName()
        {
            return branchName;
        }
        /// <summary>
        /// Quick form to set up commit message
        /// </summary>
        /// <param name="_jobNumb">Job number of the file</param>
        /// <param name="_opNumb">Operation number of the file</param>
        /// <param name="_partNumb">Part number of the file</param>
        /// <param name="_DWGRev">Revision information of the file</param>
        /// <param name="_machineNumb">The machine number that the file was run on</param>
        /// <param name="_machineProgNumb">The machine program number</param>
        /// <param name="_releaseNumb">The release number of the program</param>
        /// <param name="_toolingInfo">Tooling info for the job, set to <c>null</c> by default</param>
        /// <param name="_setupChanges"><c>Boolean</c>:Indicates whether changes were made in the job setup, set to <c>false</c> by default</param>
        /// <param name="_progChange"><c>Boolean</c>:Indicates whether changes were made in the program, set to <c>false</c> by default</param>
        /// <param name="_submitterName">The name of the person making the commit</param>
        public void BuildMsg(string _jobNumb, string _opNumb, string _partNumb, string _DWGRev, string _machineNumb, string _machineProgNumb, string _releaseNumb, string _toolingInfo = null, bool _setupChanges = false, bool _progChange = false, string _submitterName = null)
        {
            jobNumb = _jobNumb;
            opNumb = _opNumb;
            partNumb = _partNumb;
            DWGRev = _DWGRev;
            machineNumb = _machineNumb;
            machineProgNumb = _machineProgNumb;
            releaseNumb = _releaseNumb;
            toolingInfo = _toolingInfo;
            setupChanges = _setupChanges;
            submitterName = _submitterName;
            string date = DateTime.Now.ToString("MM.dd.yyyy");
            if (_progChange == true)
            {
                progChange = _progChange;
            }
            else if (_progChange == false && progChange == true)
            {
                _progChange = true;
            }
            string msg;
            if (progChange)
            {
                msg = "Changed Program";
            }
            else
            {
                msg = "Routine Submission";
            }
            msg += "\nJob #: " + jobNumb + "\nOp #: " + opNumb + "\nMachine Program #:" + machineProgNumb + "\nMachine #: " + machineNumb + " -- " + date + "\nRev: " + DWGRev + "\nRelease: " + releaseNumb + "\nCust. Part #: " + partNumb;
            if (setupChanges)
            {
                msg += "\n---Setup Changes Made---";
            }
            msg += " \n**** Tooling Info ****\n" + _toolingInfo + "\n**** End of tooling info ****";

            commitMsg = msg;
        }
        /// <summary>
        /// Set a custom commit message
        /// </summary>
        /// <param name="_commit_Msg"><c>string</c>:The message that you would like to set the commit message to</param>
        public void SetCommitMsg(string _commit_Msg)
        {
            commitMsg = _commit_Msg;
        }
        /// <summary>
        /// Get what the current commit message is
        /// </summary>
        /// <returns>Commit message</returns>
        public string GetCommitMsg()
        {
            return commitMsg;
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
}
