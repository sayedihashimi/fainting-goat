namespace fainting.goat.common {
    using Ninject;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class KernelManager {
        private static IKernel kernel = CreateNewKernel();
        private static Func<IKernel> Resolver { get; set; }

        public static IKernel GetKernel() {
            Func<IKernel> resolver = Resolver;

            IKernel result = kernel;

            if (resolver != null) {
                result = resolver();
            }

            return result;
        }

        public static void SetKernelResolver(Func<IKernel> resolver) {
            Resolver = resolver;
        }

        private static IKernel CreateNewKernel() {
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