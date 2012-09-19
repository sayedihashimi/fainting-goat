namespace fainting.goat.common {
    using NGit;
    using NGit.Api;
    using NGit.Storage.File;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IGitClient {
        /// <summary>
        /// Performs a pull on the git repository at the folder specified.
        /// </summary>
        /// <param name="gitFolder">Path to the folder where the repo lives</param>
        void PerformPull(string gitFolder);

        /// <summary>
        /// This will call <code>git clone</code> if the repo doesn't exist
        /// </summary>
        /// <param name="gitFolder">local folder path to where the .git folder should go</param>
        /// <param name="gitUri">the URL for the git repository</param>
        /// <param name="branchName">Optional: the name of the branch to be fetched. It should be specified
        /// in the verbose form, i.e. refs/heads/weekly
        /// </param>
        void InitalizeRepo(string gitFolder, string gitUri, string branchName);
    }

    public class NGitGitClient : IGitClient {
        public void PerformPull(string gitFolder) {
            if (string.IsNullOrEmpty(gitFolder)) { throw new ArgumentNullException("gitFolder"); }
            if (!Directory.Exists(gitFolder)) { throw new DirectoryNotFoundException(string.Format("git directory not found at [{0}]", gitFolder)); }

            string pathToDotGitFolder = gitFolder;
            // if this is not pointing to .git then add that to the path
            DirectoryInfo di = new DirectoryInfo(pathToDotGitFolder);
            if (string.Compare(".git", di.Name, StringComparison.OrdinalIgnoreCase) != 0) {
                pathToDotGitFolder = Path.Combine(gitFolder, @".git\");
            }

            // ensure that there is no \ at the end of the path
            pathToDotGitFolder = pathToDotGitFolder.Trim().TrimEnd('\\');

            FileRepositoryBuilder builder = new FileRepositoryBuilder();
            Repository repo = builder
                                .SetGitDir(pathToDotGitFolder)
                                .ReadEnvironment()
                                .FindGitDir()
                                .Build();

            Git git = new Git(repo);
            PullCommand pullCommand = git.Pull();
            PullResult pullResult = null;

            // TODO: need to cover this in a try/catch and log errors
            pullResult = pullCommand.Call();
        }

        public void InitalizeRepo(string gitFolder, string gitUri, string branchName) {
            if (string.IsNullOrEmpty(gitFolder)) { throw new ArgumentNullException("gitFolder"); }
            if (string.IsNullOrEmpty(gitUri)) { throw new ArgumentNullException("gitUri"); }

            // TODO: need to cover this in a try/catch and log errors
            Task cloneTask = new Task(() => {
                CloneCommand cloneCommand = Git.CloneRepository();
                cloneCommand.SetBare(false);
                cloneCommand.SetDirectory(gitFolder);
                cloneCommand.SetURI(gitUri);

                if (!string.IsNullOrEmpty(branchName)) {
                    cloneCommand.SetCloneAllBranches(false);
                    cloneCommand.SetBranchesToClone(new string[] { branchName });
                    cloneCommand.SetBranch(branchName);
                }

                cloneCommand.Call();
            });

            cloneTask.Start();
            cloneTask.Wait();
        }
    }
}