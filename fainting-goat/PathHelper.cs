using fainting.goat.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fainting.goat {
    public class PathHelper {
        public PathHelper(IConfig config) {
            if (config == null) { throw new ArgumentNullException("config"); }

            this.Config = config;
        }

        private IConfig Config { get; set; }

        internal string ConvertMdUriToLocalPath(HttpContextBase httpContext, string path) {
            if (httpContext == null) { throw new ArgumentNullException("httpContext"); }
            if (string.IsNullOrEmpty(path)) { throw new ArgumentNullException("path"); }

            // convert the url path /foo/bar/page.md to \foo\bar\page.md
            string localRepoFolderUri = this.Config.GetConfigValue(CommonConsts.AppSettings.MarkdownSourceFolder);
            string pathToConvert = string.Format("{0}{1}", localRepoFolderUri, path);

            string localPath = httpContext.Server.MapPath(pathToConvert);

            return localPath;
        }

        internal string ConvertMdUriToLocalPath(HttpContext httpContext, string path) {
            if (httpContext == null) { throw new ArgumentNullException("httpContext"); }
            if (string.IsNullOrEmpty(path)) { throw new ArgumentNullException("path"); }

            // convert the url path /foo/bar/page.md to \foo\bar\page.md
            string localRepoFolderUri = this.Config.GetConfigValue(CommonConsts.AppSettings.MarkdownSourceFolder);
            string pathToConvert = string.Format("{0}{1}", localRepoFolderUri, path);

            string localPath = httpContext.Server.MapPath(pathToConvert);

            return localPath;
        }
    }
}