namespace fainting.goat {
    using fainting.goat.App_Start;
    using fainting.goat.common;
    using Ninject;
    using Ninject.Web.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            IKernel kernel = this.RegisterNinject();
            this.UpdateGitRepo(kernel);

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private IKernel RegisterNinject() {
            var kernel = new StandardKernel();
            
            kernel.Bind<IMarkdownToHtml>().To<MarkdownSharpMarkdownToHtml>();
            kernel.Bind<IContentRepository>().To<FileContentRepository>();
            kernel.Bind<IConfigHelper>().To<ConfigHelper>();

            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));

            return kernel;
        }

        private void UpdateGitRepo(IKernel kernel) {
            if (kernel == null) { throw new ArgumentNullException("kernel"); }

            Task updateGitRepo = new Task(() => {
                IConfigHelper cfgHelper = kernel.Get<IConfigHelper>();

                string localPath = this.Context.Server.MapPath(cfgHelper.GetConfigValue(CommonConsts.AppSettings.MarkdownSourceFolder));

                new GitConfig().CreateNewGitClient().PerformPull(localPath);
                string debug = "dd";
            });

            updateGitRepo.Start();
        }
    }
}