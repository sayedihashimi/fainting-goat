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
        public MarkdownController(IConfig config, IMarkdownToHtml markdownToHtml, IContentRepository contentRepo, GitHelper gitHelper, PathHelper pathHelper) :
            base(config, markdownToHtml, contentRepo, gitHelper, pathHelper)
        {
        }

        public ActionResult Index() {
            return View(@"/Views/Markdown/Render.cshtml",
                this.MakeMarkDownViewModel("index.md")
                );
        }

        public ActionResult Render(string mdroute)
        {
            return View(this.MakeMarkDownViewModel(mdroute));
        }

        private MarkdownPageModel MakeMarkDownViewModel(string mdroute) {
            string localPath = this.PathHelper.ConvertMdUriToLocalPath(mdroute, (s) => Server.MapPath(s));
            string md = this.ContentRepo.GetContentFor(new Uri(localPath));

            MarkdownPageModel pm = new MarkdownPageModel {
                HtmlToRender = this.MarkdownToHtml.ConvertToHtml(md)
            };

            return pm;
        }

        public string UpdateRepo()
        {
            GitHelper.UpdateGitRepo(Server.MapPath(Config.GetConfigValue(CommonConsts.AppSettings.MarkdownSourceFolder)));
            return "Updating";
        }
    }
}
