using fainting.goat.common;
using System;

namespace fainting.goat.common {
    public class PathHelper {
        public PathHelper(IConfig config) {
            if(config == null) { throw new ArgumentNullException("config"); }
            this.Config = config;
        }

        private IConfig Config { get; set; }

        public string ConvertMdUriToLocalPath(string path, Func<string,string> pathCleansingCallback) {
            if(string.IsNullOrEmpty(path)) { throw new ArgumentNullException("path"); }

            // build the VIRTUAL path to the file from what's in "Web.config + SomeFile.md"
            string localRepoFolderUri = this.Config.GetConfigValue(CommonConsts.AppSettings.MarkdownSourceFolder);
            string pathToConvert = string.Format("{0}{1}", localRepoFolderUri, path);

            // if the calling code has some sort of cleansing, like converting a virtual path to a physical path, allow for that customization here
            string localPath = (pathCleansingCallback != null)
                ? pathCleansingCallback(pathToConvert)
                : pathToConvert;

            return localPath;
        }
    }
}