namespace fainting.goat.App_Start {
    using fainting.goat.common;
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class NinjectConfig {
        public IKernel CreateKernel() {
            IKernel kernel = new StandardKernel();

            kernel.Bind<IMarkdownToHtml>().To<MarkdownSharpMarkdownToHtml>();
            kernel.Bind<IContentRepository>().To<FileContentRepository>();
            kernel.Bind<IConfig>().To<Config>();

            return kernel;
        }
    }
}