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
        public HomeController(IConfigHelper configHelper, IMarkdownToHtml markdownToHtml, IContentRepository contentRepo) :
            base(configHelper, markdownToHtml, contentRepo) {
        }

        public ActionResult Index()
        {
            string localPath = this.PathHelper.ConvertMdUriToLocalPath(this.HttpContext, "index.md");
            string md = this.ContentRepo.GetContentFor(new Uri(localPath));

            MarkdownPageModel pm = new MarkdownPageModel {
                HtmlToRender = this.MarkdownToHtml.ConvertToHtml(md)
            };

            return View(@"/Views/Markdown/Render.cshtml", pm);
        }
    }
}
