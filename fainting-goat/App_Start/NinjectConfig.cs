namespace fainting.goat {
    using fainting.goat.common;
    using Ninject;
    using System;
    using System.Web;

    public class NinjectConfig {
        public IKernel CreateKernel() {
            IKernel kernel = new StandardKernel();

            kernel.Bind<IMarkdownToHtml>().To<MarkdownSharpMarkdownToHtml>();
            kernel.Bind<IContentRepository>().To<FileContentRepository>();
            kernel.Bind<IConfig>().To<Config>();
            kernel.Bind<IGitClient>().To<NGitGitClient>();
            kernel.Bind<IPathHelper>().To<PathHelper>();

            return kernel;
        }
    }
}