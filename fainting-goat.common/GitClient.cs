namespace fainting.goat.common {
    using NGit;
    using NGit.Api;
    using NGit.Storage.File;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public interface IGitClient {
        /// <summary>
        /// Performs a pull on the git repository at the folder specified.
        /// </summary>
        /// <param name="gitFolder">Path to the folder where the repo lives</param>
        void PerformPull(string gitFolder);
    }

    public class NGitGitClient : IGitClient {
        public void PerformPull(string gitFolder) {
            if (string.IsNullOrEmpty(gitFolder)) { throw new ArgumentNullException("gitFolder"); }
            if (!Directory.Exists(gitFolder)) { throw new DirectoryNotFoundException(string.Format("git directory not found at [{0}]",gitFolder)); }

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
            PullResult pullResult = pullCommand.Call();
        }
    }
}