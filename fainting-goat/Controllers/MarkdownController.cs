﻿namespace fainting.goat.Controllers
{
    using fainting.goat.common;
    using fainting.goat.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class MarkdownController : MarkdownBaseController
    {
        public MarkdownController(IConfig config, IMarkdownToHtml markdownToHtml, IContentRepository contentRepo, GitHelper gitHelper) :
            base(config, markdownToHtml, contentRepo, gitHelper)
        {
        }

        public ActionResult Render(string mdroute)
        {
            MarkdownPageModel pm = new MarkdownPageModel();

            string localPath = this.PathHelper.ConvertMdUriToLocalPath(mdroute, (s) => Server.MapPath(s));
            string md = this.ContentRepo.GetContentFor(new Uri(localPath));

            pm.HtmlToRender = this.MarkdownToHtml.ConvertToHtml(md);

            return View(pm);
        }

        public string UpdateRepo()
        {
            GitHelper.UpdateGitRepo(Server.MapPath(Config.GetConfigValue(CommonConsts.AppSettings.MarkdownSourceFolder)));

            return "Updating";
        }

        public class HtmlResult : ActionResult
        {
            public HtmlResult(string html)
            {
                if (html == null) { throw new ArgumentNullException("html"); }
                this.Html = html;
            }

            private string Html { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                HttpContextBase httpContextBase = context.HttpContext;

                httpContextBase.Response.Write(this.Html);
            }
        }
    }
}
