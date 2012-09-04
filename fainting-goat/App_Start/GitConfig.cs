namespace fainting.goat.App_Start {
    using fainting.goat.common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class GitConfig {

        public IGitClient CreateNewGitClient() {
            return new NGitGitClient();
        }
    }
}