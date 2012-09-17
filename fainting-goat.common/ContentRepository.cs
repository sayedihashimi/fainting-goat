namespace fainting.goat.common {
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Will return the contents of the item at the given path
    /// </summary>
    public interface IContentRepository {
        string GetContentFor(Uri uri);
    }

    public class FileContentRepository : IContentRepository {
        private IPathHelper _pathHelper;

        public FileContentRepository(IPathHelper pathHelper) {
            _pathHelper = pathHelper;
        }

        public string GetContentFor(Uri uri) {
            if(uri == null) { throw new ArgumentNullException("uri"); }
            if(!uri.IsFile) {
                throw new ArgumentException(string.Format("FileContentRepository only supports URIs pointing to files, the URI provided is not a valid file URI"));
            }

            // TODO: This could be optimized to read the file in a streaming manner
            string contents = File.ReadAllText(uri.LocalPath);

            return contents;
        }
    }
}
