namespace fainting.goat.Controllers
{
    using fainting.goat.common;
    using fainting.goat.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class HomeController : MarkdownBaseController
    {
        public HomeController(IConfig config, IMarkdownToHtml markdownToHtml, IContentRepository contentRepo, GitHelper gitHelper, PathHelper pathHelper) :
            base(config, markdownToHtml, contentRepo, gitHelper, pathHelper)
        {
        }

        public ActionResult Index()
        {
            string localPath = this.PathHelper.ConvertMdUriToLocalPath("index.md", (s) => Server.MapPath(s));
            string md = this.ContentRepo.GetContentFor(new Uri(localPath));

            MarkdownPageModel pm = new MarkdownPageModel {
                HtmlToRender = this.MarkdownToHtml.ConvertToHtml(md)
            };

            return View(@"/Views/Markdown/Render.cshtml", pm);
        }
    }
}
