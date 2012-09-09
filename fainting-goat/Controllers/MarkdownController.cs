namespace fainting.goat.Controllers {
    using fainting.goat.common;
    using fainting.goat.Models;
    using System;
    using System.IO;
    using System.Web.Mvc;

    public class MarkdownController : MarkdownBaseController {
        public MarkdownController(IConfig config, IMarkdownToHtml markdownToHtml, IContentRepository contentRepo, GitHelper gitHelper, PathHelper pathHelper) :
            base(config, markdownToHtml, contentRepo, gitHelper, pathHelper) {
        }

        public ActionResult Index() {
            return View(@"/Views/Markdown/Render.cshtml",
                this.MakeMarkDownViewModelFromLocalPath(this.GetDefaultDocumentFullLocalPath()));
        }

        public ActionResult Render(string mdroute) {
            return View(this.MakeMarkDownViewModel(mdroute));
        }

        private MarkdownPageModel MakeMarkDownViewModelFromLocalPath(string localPath) {
            if (string.IsNullOrEmpty(localPath)) { throw new ArgumentNullException("localPath"); }
            if (!System.IO.File.Exists(localPath)) { throw new FileNotFoundException(string.Format("Markdown file not found at [{0}]", localPath)); }

            string md = this.ContentRepo.GetContentFor(new Uri(localPath));
            MarkdownPageModel pm = new MarkdownPageModel {
                FaintingGoatWebTitle = this.GetTitle(),
                HtmlToRender = this.MarkdownToHtml.ConvertToHtml(md),
                NavHtml = this.GetNavHtml()
            };

            return pm;
        }

        private MarkdownPageModel MakeMarkDownViewModel(string mdroute) {
            string localPath = this.PathHelper.ConvertMdUriToLocalPath(mdroute, (s) => Server.MapPath(s));
            if (!System.IO.File.Exists(localPath)) { throw new FileNotFoundException(string.Format("Markdown file not found at [{0}]", localPath)); }

            string md = this.ContentRepo.GetContentFor(new Uri(localPath));

            MarkdownPageModel pm = new MarkdownPageModel {
                FaintingGoatWebTitle = this.GetTitle(),
                HtmlToRender = this.MarkdownToHtml.ConvertToHtml(md),
                NavHtml = this.GetNavHtml()
            };

            return pm;
        }

        public string UpdateRepo() {
            GitHelper.UpdateGitRepo(Server.MapPath(Config.GetConfigValue(CommonConsts.AppSettings.MarkdownSourceFolder)));
            return "Updating";
        }
    }
}