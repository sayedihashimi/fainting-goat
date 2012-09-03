namespace fainting.goat.common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Will return the contents of the item at the given path
    /// </summary>
    public interface IContentRepository {
        string GetContentFor(Uri uri);
    }

    public class FileContentRepository : IContentRepository {
        public string GetContentFor(Uri uri) {
            throw new NotImplementedException();
        }
    }
}
