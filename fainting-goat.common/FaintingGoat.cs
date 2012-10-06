namespace fainting.goat.common {
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;

    public class FaintingGoat {
        public FaintingGoat(IConfig config, string localRepoPath) {
            this.Config = config;
            this.LocalRepoPath = localRepoPath;
        }

        public IConfig Config { get; set; }
        public string LocalRepoPath { get; set; }

        public void Initalize() {
            this.GetContentProvider().Initalize();
        }

        public void Update() {
            this.GetContentProvider().UpdateContents();
        }

        protected internal IMdContentProvider GetContentProvider() {
            // see if git is being used or just the file system
            // if the config has a value for gitUri then its git

            IMdContentProvider result = null;

            MdContentProviderType mdProviderType = fainting.goat.common.Config.GetContentProviderTypeFromConfig(this.Config);
            if (mdProviderType == MdContentProviderType.Git) {

                result = new GitMdContentProvier(
                    KernelManager.GetKernel().Get<GitHelper>(),
                    this.Config,
                    this.LocalRepoPath);
            }
            else if (mdProviderType == MdContentProviderType.FileSystem) {
                result = new FileSystemMdContentProvider();
            }
            else {
                string message = string.Format("Unknown MdContentProviderType [{0}]", mdProviderType);
                // TODO: Change provider type here
                throw new ApplicationException(message);
            }

            return result;
        }
    }
}
