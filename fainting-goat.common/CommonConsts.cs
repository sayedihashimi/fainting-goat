namespace fainting.goat.common {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class CommonConsts {
        public class AppSettings {
            public const string MarkdownSourceFolder = "markdownSourceFolder";
            public const string GitUri = "gitUri";
            public const string FaintingGoatWebTitle = "faintingGoatWebTitle";
            public const string DefaultDocList = "defaultDocList";

            public class DefaultValues {
                public const string FaintingGoatWebTitle = "Project Documentation";
                public static readonly IList<string> DefaultDocList = new List<string> { "index.md", "readme.md" };
            }
        }
    }
}
