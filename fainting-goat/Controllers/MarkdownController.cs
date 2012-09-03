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
        //
        // GET: /Markdown/
        public ActionResult Render(string mdroute)
        {
            MarkdownPageModel pm = new MarkdownPageModel();
            pm.HtmlToRender = string.Format(@"<h1>{0}</h1><b>from Render()</b>", mdroute);
            // return View(pm);
            return new HtmlResult(pm.HtmlToRender);
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
