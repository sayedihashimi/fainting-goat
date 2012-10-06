using fainting.goat.common;
using System;
using System.Text.RegularExpressions;

namespace fainting.goat.common {

    public interface IPathHelper {
        string ConvertMdUriToLocalPath(string path, Func<string, string> pathCleansingCallback);
    }

    public class PathHelper : IPathHelper {
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

        public static string PathCleaner(string path,Func<string,string>webPathCleaner) {
            if (string.IsNullOrEmpty(path)) { throw new ArgumentNullException("path"); }

            string result = null;
            // if it doesn't start with ~ or / assume its a full directory path and just return it
            string pattern = @"^\s*[~\\/]";

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);

            if (regex.IsMatch(path)) {
                result = path;
            }
            else {
                result = webPathCleaner(path);
            }

            return result;
        }

    }
}