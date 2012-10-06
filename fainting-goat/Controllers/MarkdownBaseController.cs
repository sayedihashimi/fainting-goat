namespace fainting.goat.Controllers
{
    using fainting.goat.common;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    [ConvertFileNotFoundTo404]
    public abstract class MarkdownBaseController : Controller
    {
        public MarkdownBaseController(IConfig config, 
            IMarkdownToHtml markdownToHtml, 
            IContentRepository contentRepo, 
            GitHelper gitHelper,
            IPathHelper pathHelper
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
        protected IPathHelper PathHelper { get; set; }
        protected GitHelper GitHelper { get; set; }

        protected string GetDefaultDocumentFullLocalPath() {
            IList<string> defaultDocListConfig = this.Config.GetList(CommonConsts.AppSettings.DefaultDocList, defaultValue: CommonConsts.AppSettings.DefaultValues.DefaultDocList);

            string result = null;
            FullPathCleaner cleaner = new FullPathCleaner(s => Server.MapPath(s));

            for (int i = 0; i < defaultDocListConfig.Count; i++) {
                string fileName = defaultDocListConfig[i];

                string fileNameLocalpath = this.PathHelper.ConvertMdUriToLocalPath(
                    fileName,
                    s => cleaner.CleanPath(s));

                if (System.IO.File.Exists(fileNameLocalpath)) {
                    result = fileNameLocalpath;
                    break;
                }
            }

            return result;
        }

        protected string GetHeaderHtml() {
            IList<string> files = this.Config.GetList(CommonConsts.AppSettings.HeaderFiles, defaultValue: CommonConsts.AppSettings.DefaultValues.HeaderFiles);

            string header = this.GetHtmlFromFirstFound(files);
            if (header == null) {
                header = CommonConsts.AppSettings.DefaultValues.HeaderHtml;
            }
            return header;
        }

        protected string GetFooterHtml() {
            IList<string> files = this.Config.GetList(CommonConsts.AppSettings.FooterFiles, defaultValue: CommonConsts.AppSettings.DefaultValues.FooterFiles);
            return this.GetHtmlFromFirstFound(files);
        }

        protected string GetHtmlFromFirstFound(IEnumerable<string> filesToCheck) {
            if (filesToCheck == null) { throw new ArgumentNullException("filesToCheck"); }
            string result = null;

            List<string> filesToTryRelative = new List<string>(filesToCheck);
            List<string>filesToTryAbsolute = new List<string>();

            FullPathCleaner cleaner = new FullPathCleaner(s => Server.MapPath(s));
            
            filesToTryRelative.ForEach(file=>{
                filesToTryAbsolute.Add(this.PathHelper.ConvertMdUriToLocalPath(file,(s)=>cleaner.CleanPath(s)));
            });


            string currentFile = null;

            foreach (var nf in filesToTryAbsolute) {
                if (System.IO.File.Exists(nf)) {
                    currentFile = nf;
                    break;
                }
            }

            if (currentFile != null) {
                string md = this.ContentRepo.GetContentFor(new Uri(currentFile));
                result = this.MarkdownToHtml.ConvertToHtml(md);
            }

            return result;
        }

        protected string GetTitle() {
            string titleFromConfig = this.Config.GetConfigValue(CommonConsts.AppSettings.FaintingGoatWebTitle);

            return titleFromConfig != null ? titleFromConfig : CommonConsts.AppSettings.DefaultValues.FaintingGoatWebTitle;
        }
    }
}
