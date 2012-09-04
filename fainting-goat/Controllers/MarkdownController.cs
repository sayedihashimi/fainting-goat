using fainting.goat.common;
using fainting.goat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace fainting.goat.Controllers
{
    public class MarkdownController : Controller
    {
        public MarkdownController(IConfigHelper configHelper, IMarkdownToHtml markdownToHtml,IContentRepository contentRepo) {
            if (configHelper == null) { throw new ArgumentNullException("configHelper"); }
            if (markdownToHtml == null) { throw new ArgumentNullException("markdownToHtml"); }
            if (contentRepo == null) { throw new ArgumentNullException("contentRepo"); }

            this.ConfigHelper = configHelper;
            this.MarkdownToHtml = markdownToHtml;
            this.ContentRepo = contentRepo;
        }

        private IMarkdownToHtml MarkdownToHtml { get; set; }
        private IContentRepository ContentRepo { get; set; }
        private IConfigHelper ConfigHelper { get; set; }

        //
        // GET: /Markdown/
        public ActionResult Render(string mdroute)
        {
            MarkdownPageModel pm = new MarkdownPageModel();

            string md = this.GetMarkdownFor(mdroute);

            pm.HtmlToRender = this.MarkdownToHtml.ConvertToHtml(md);

            return View(pm);
        }

        protected string GetMarkdownFor(string path) {
            if (string.IsNullOrEmpty(path)) { throw new ArgumentNullException("path"); }

            string localPath = this.ConvertMdUriToLocalPath(path);

            return this.ContentRepo.GetContentFor(new Uri(localPath));
        }

        private string ConvertMdUriToLocalPath(string path) {
            if (string.IsNullOrEmpty(path)) { throw new ArgumentNullException("path"); }

            // convert the url path /foo/bar/page.md to \foo\bar\page.md
            string localRepoFolderUri = this.ConfigHelper.GetConfigValue(CommonConsts.AppSettings.MarkdownSourceFolder);
            string pathToConvert = string.Format("{0}{1}", localRepoFolderUri, path);

            string localPath = this.HttpContext.Server.MapPath(pathToConvert);

            return localPath;
        }


        public class HtmlResult : ActionResult {
            public HtmlResult(string html){
                if (html == null) { throw new ArgumentNullException("html"); }
                this.Html = html;
            }

            private string Html { get; set; }

            public override void ExecuteResult(ControllerContext context) {
                HttpContextBase httpContextBase = context.HttpContext;
                
                httpContextBase.Response.Write(this.Html);
            }
        }
    }
}
