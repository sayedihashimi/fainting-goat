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
        public MarkdownController(IMarkdownToHtml markdownToHtml,IContentRepository contentRepo) {
            if (markdownToHtml == null) { throw new ArgumentNullException("markdownToHtml"); }
            if (contentRepo == null) { throw new ArgumentNullException("contentRepo"); }

            this.MarkdownToHtml = markdownToHtml;
        }

        private IMarkdownToHtml MarkdownToHtml { get; set; }
        private IContentRepository ContentRepo { get; set; }

        //
        // GET: /Markdown/
        public ActionResult Render(string mdroute)
        {
            MarkdownPageModel pm = new MarkdownPageModel();

            string sampleMarkdown =
@"At first, I tried to create a RegEx for empty string which is `^$` (which is what null would be). However, it doesn't look like route constraints can be `!=`. How about matching one or more character with `^.+$`? Here is my website http://sedodream.com and you can email me at sayedha@microsoft.com.

So:

    tag = @""^.+$""
    tag = @""^.+$""";

            pm.HtmlToRender = this.MarkdownToHtml.ConvertToHtml(sampleMarkdown);
            return View(pm);
        }

        protected string GetMarkdownFor(string path) {
            if (string.IsNullOrEmpty(path)) { throw new ArgumentNullException("path"); }

            // convert the url path /foo/bar/page.md to \foo\bar\page.md


            throw new NotImplementedException();
        }

        private string ConvertMdUriToLocalPath(string uri) {
            // convert the url path /foo/bar/page.md to \foo\bar\page.md

            throw new NotImplementedException();
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
