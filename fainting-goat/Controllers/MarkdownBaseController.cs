namespace fainting.goat.Controllers
{
    using fainting.goat.common;
    using fainting.goat.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public abstract class MarkdownBaseController : Controller
    {
        public MarkdownBaseController(IConfig config, 
            IMarkdownToHtml markdownToHtml, 
            IContentRepository contentRepo, 
            GitHelper gitHelper,
            PathHelper pathHelper
            )
        {
            if (config == null) { throw new ArgumentNullException("config"); }
            if (markdownToHtml == null) { throw new ArgumentNullException("markdownToHtml"); }
            if (contentRepo == null) { throw new ArgumentNullException("contentRepo"); }

            this.Config = config;
            this.MarkdownToHtml = markdownToHtml;
            this.ContentRepo = contentRepo;
            this.PathHelper = pathHelper;
            this.GitHelper = gitHelper;
        }

        protected IMarkdownToHtml MarkdownToHtml { get; set; }
        protected IContentRepository ContentRepo { get; set; }
        protected IConfig Config { get; set; }
        protected PathHelper PathHelper { get; set; }
        protected GitHelper GitHelper { get; set; }

        protected string GetNavHtml() {
            string result = string.Empty;

            List<string> navFilesToTry = new List<string> {
                this.PathHelper.ConvertMdUriToLocalPath("nav.md", (s) => Server.MapPath(s)),
                this.PathHelper.ConvertMdUriToLocalPath("nav.markdown", (s) => Server.MapPath(s)),
                this.PathHelper.ConvertMdUriToLocalPath("nav.mdown", (s) => Server.MapPath(s))
            };

            string navFile = null;

            foreach (var nf in navFilesToTry) {
                if (System.IO.File.Exists(nf)) {
                    navFile = nf;
                    break;
                }
            }

            if (navFile != null) {
                string md = this.ContentRepo.GetContentFor(new Uri(navFile));
                result = this.MarkdownToHtml.ConvertToHtml(md);
            }

            return result;
        }

        protected string GetTitle() {
            string titleFromConfig = this.Config.GetConfigValue(CommonConsts.AppSettings.FaintingGoatWebTitle);

            return titleFromConfig != null ? titleFromConfig : "Project documentation";
        }
    }
}
