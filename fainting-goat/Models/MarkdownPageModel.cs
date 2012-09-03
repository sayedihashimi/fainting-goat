using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace fainting.goat.Models {
    public class MarkdownPageModel {
        public MarkdownPageModel() {
            this.HtmlToRender = string.Empty;
        }
        public string HtmlToRender { get; set; }
    }
}