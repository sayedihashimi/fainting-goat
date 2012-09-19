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
            public const string HeaderFiles = "headerFiles";
            public const string FooterFiles = "footerFiles";
            public const string GitBranchName = "gitBranchName";

            public class DefaultValues {
                public const string FaintingGoatWebTitle = "Project Documentation";
                public static readonly IList<string> DefaultDocList = new List<string> { "index.md", "readme.md" };
                public static readonly IList<string> HeaderFiles = new List<string> { "header.md", "header.markdown", "header.mdown" };
                public static readonly IList<string> FooterFiles = new List<string> {"footer.md","footer.markdown","footer.mdown"};
                
                public const string HeaderHtml =
@"<header id=""topHeader"">
    <section id=""headerLogo"">
        <span>project doc site</span>
    </section>
</header>";
            }
        }
    }
}
