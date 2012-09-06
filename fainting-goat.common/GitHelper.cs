namespace fainting.goat.common
{
    using fainting.goat.common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    public class GitHelper
    {
        private IConfig _config;
        private IGitClient _gitClient;

        public GitHelper(IConfig config, IGitClient gitClient)
        {
            _config = config;
            _gitClient = gitClient;
        }

        public void UpdateGitRepo(string path)
        {
            if (_config == null) { throw new ArgumentNullException("config"); }

            Task updateGitRepo = new Task(() =>
            {
                _gitClient.PerformPull(path);
            });

            updateGitRepo.Start();
        }
    }
}