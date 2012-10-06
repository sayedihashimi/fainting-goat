namespace fainting.goat
{
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

    public class MvcApplication : System.Web.HttpApplication
    {
        static internal IKernel Kernel { get; set; }
        static object KernelLock = new object();

        public MvcApplication() {
            if (MvcApplication.Kernel == null) {
                lock (KernelLock) {
                    if (MvcApplication.Kernel == null) {
                        MvcApplication.Kernel = this.RegisterNinject();

                        KernelManager.SetKernelResolver(() => { return MvcApplication.Kernel; });
                    }
                }
            }
        }
        
        protected void Application_Start()
        {
            this.UpdateGitRepo(MvcApplication.Kernel);

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private IKernel RegisterNinject()
        {
            IKernel kernel = new NinjectConfig().CreateKernel();

            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));

            return kernel;
        }

        private void UpdateGitRepo(IKernel kernel)
        {
            if (kernel == null) { throw new ArgumentNullException("kernel"); }
            FullPathCleaner cleaner = new FullPathCleaner(s => Server.MapPath(s));
            string repoPath = cleaner.CleanPath(
                kernel.Get<IConfig>().GetConfigValue(
                        CommonConsts.AppSettings.MarkdownSourceFolder));


            new FaintingGoat(kernel.Get<IConfig>(), repoPath).Update();
        }
    }
}