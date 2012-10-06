namespace fainting.goat.common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public enum MdContentProviderType {
        Git,
        FileSystem
    }

    public interface IMdContentProvider {
        MdContentProviderType ProviderType { get; }
        void Initalize();
        void UpdateContents();
    }

    /// <summary>
    /// This class doesn't have to do aything, the files are already on disk and ready to be served
    /// later we may use this class to perform some work if neeeded.
    /// </summary>
    public class FileSystemMdContentProvider :  IMdContentProvider {
        public MdContentProviderType ProviderType { get { return MdContentProviderType.FileSystem; } }
        public void Initalize() { }
        public void UpdateContents() { }
    }

    public class GitMdContentProvier : IMdContentProvider {

        public GitMdContentProvier(GitHelper gitHelper, IConfig config, string localRepoPath) {
            if (config == null) { throw new ArgumentNullException("config"); }
            if (string.IsNullOrEmpty(localRepoPath)) { throw new ArgumentNullException("localRepoPath"); }

            this.GitHelper = gitHelper;
            this.Config = config;
            this.LocalRepoPath = localRepoPath;
        }

        public MdContentProviderType ProviderType { get { return MdContentProviderType.Git; } }
        private GitHelper GitHelper { get; set; }
        private IConfig Config { get; set; }
        private string LocalRepoPath { get; set; }

        /// <summary>
        /// For the git provider this will perform a clone/pull based on if the repo has previously been cloned or not
        /// </summary>
        public void Initalize() {
            this.UpdateContents();
        }

        /// <summary>
        /// For the git provider this will perform a pull. The repo should already be cloned locally.
        /// </summary>
        public void UpdateContents() {
            this.GitHelper.UpdateGitRepo(
                this.LocalRepoPath,
                this.Config.GetConfigValue(CommonConsts.AppSettings.GitBranchName));
        }
    }
}
