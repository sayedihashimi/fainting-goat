namespace fainting.goat {
    using fainting.goat.App_Start;
    using fainting.goat.common;
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    public class GitHelper {
        public void UpdateGitRepo(IConfig config, HttpContext httpContext) {
            if (config == null) { throw new ArgumentNullException("config"); }
            if (httpContext == null) { throw new ArgumentNullException("httpContext"); }

            Task updateGitRepo = new Task(() => {
                string localPath = httpContext.Server.MapPath(config.GetConfigValue(CommonConsts.AppSettings.MarkdownSourceFolder));
                new GitConfig().CreateNewGitClient().PerformPull(localPath);
            });

            updateGitRepo.Start();
        }

        public void UpdateGitRepo(IConfig config, HttpContextBase httpContext) {
            if (config == null) { throw new ArgumentNullException("config"); }
            if (httpContext == null) { throw new ArgumentNullException("httpContext"); }

            Task updateGitRepo = new Task(() => {
                string localPath = httpContext.Server.MapPath(config.GetConfigValue(CommonConsts.AppSettings.MarkdownSourceFolder));
                new GitConfig().CreateNewGitClient().PerformPull(localPath);
            });

            updateGitRepo.Start();
        }
    }
}