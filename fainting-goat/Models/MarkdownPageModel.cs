using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fainting.goat.Models {
    public class MarkdownPageModel {
        public MarkdownPageModel() {
            this.HtmlToRender = string.Empty;
            this.NavHtml = string.Empty;
        }

        public string HtmlToRender { get; set; }
        public string NavHtml { get; set; }
        public string FaintingGoatWebTitle { get; set; }
    }
}