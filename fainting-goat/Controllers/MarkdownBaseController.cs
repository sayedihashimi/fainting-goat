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
        public MarkdownBaseController(IConfigHelper configHelper, IMarkdownToHtml markdownToHtml, IContentRepository contentRepo) {
            if (configHelper == null) { throw new ArgumentNullException("configHelper"); }
            if (markdownToHtml == null) { throw new ArgumentNullException("markdownToHtml"); }
            if (contentRepo == null) { throw new ArgumentNullException("contentRepo"); }

            this.ConfigHelper = configHelper;
            this.MarkdownToHtml = markdownToHtml;
            this.ContentRepo = contentRepo;
            this.PathHelper = new PathHelper(this.ConfigHelper);
        }

        protected IMarkdownToHtml MarkdownToHtml { get; set; }
        protected IContentRepository ContentRepo { get; set; }
        protected IConfigHelper ConfigHelper { get; set; }
        protected PathHelper PathHelper { get; set; }
    }
}
