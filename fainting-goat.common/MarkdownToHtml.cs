namespace fainting.goat.common {
    using MarkdownSharp;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public interface IMarkdownToHtml {
        string ConvertToHtml(string markdown);
        string ConvertToHtmlFromFile(string filepath);
    }

    public class MarkdownSharpMarkdownToHtml : IMarkdownToHtml {

        public MarkdownSharpMarkdownToHtml() {
            MarkdownOptions options = new MarkdownOptions();
            options.AutoHyperlink = true;
            // options.AutoNewLines = true;
            options.LinkEmails = true;

            this.Markdown = new Markdown(options);
        }

        private Markdown Markdown { get; set; }

        public string ConvertToHtml(string markdown) {
            if (string.IsNullOrEmpty(markdown)) { throw new ArgumentNullException("markdown"); }

            return this.Markdown.Transform(markdown);
        }

        public string ConvertToHtmlFromFile(string filepath) {
            if (string.IsNullOrEmpty(filepath)) { throw new ArgumentNullException("filepath"); }
            if (!File.Exists(filepath)) { throw new FileNotFoundException(string.Format("Markdown file not found at [{0}]", filepath)); }

            // TODO: Read the file by streaming content to a StringBuilder
            string fileContents = File.ReadAllText(filepath);

            return this.ConvertToHtml(fileContents);
        }
    }
}
