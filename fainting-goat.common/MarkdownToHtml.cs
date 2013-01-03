namespace fainting.goat.common {
    using MarkdownSharp;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

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

            string html = this.Markdown.Transform(markdown);

            if (html != null) {
                html = html.Trim();
            }
            if (html.StartsWith(@"<p>", StringComparison.OrdinalIgnoreCase) && html.EndsWith(@"</p>", StringComparison.OrdinalIgnoreCase)) {
                html = html.Substring(3, html.Length - 7);
            }

            return html;
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
