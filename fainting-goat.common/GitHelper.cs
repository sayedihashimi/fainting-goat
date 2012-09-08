namespace fainting.goat.common
{
    using fainting.goat.common;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    public class GitHelper
    {
        private IConfig _config;
        private IGitClient _gitClient;

        public GitHelper(IConfig config, IGitClient gitClient)
        {
            if (config == null) { throw new ArgumentNullException("config"); }
            if (gitClient == null) { throw new ArgumentNullException("gitClient"); }

            _config = config;
            _gitClient = gitClient;
        }

        public void UpdateGitRepo(string path)
        {
            if (string.IsNullOrEmpty(path)) { throw new ArgumentNullException("path"); }            
            if (_config == null) { throw new ArgumentNullException("config"); }

            if (!Directory.Exists(path)) {
                this._gitClient.InitalizeRepo(path, this._config.GetConfigValue(CommonConsts.AppSettings.GitUri, isRequired: true));
            }
            else {
                Task updateGitRepo = new Task(() => {
                    _gitClient.PerformPull(path);
                });

                updateGitRepo.Start();
            }
        }
    }
}