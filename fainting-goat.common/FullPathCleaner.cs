namespace fainting.goat.common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class FullPathCleaner {
        public FullPathCleaner(Func<string, string> secondaryPathCleaner) {
            this.WebPathCleaner = secondaryPathCleaner;
        }

        public Func<string, string> WebPathCleaner { get; private set; }

        public string CleanPath(string path) {
            if (string.IsNullOrEmpty(path)) { throw new ArgumentNullException("path"); }

            string result = null;
            // if it doesn't start with ~ or / assume its a full directory path and just return it
            string webFolderPattern = @"^\s*[~\\/]";

            Regex regex = new System.Text.RegularExpressions.Regex(webFolderPattern);

            if (regex.IsMatch(path)) {
                result = this.WebPathCleaner(path);                
            }
            else {
                result = path;
            }

            return result;
        }
    }
}
