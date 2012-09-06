using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace fainting.goat {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "markdown",
                url: "{*mdroute}",
                defaults: new { controller = "Markdown", action = "Render" },
                constraints: new { mdroute = @".+\.md$" }
                );

            routes.MapRoute(
                name: "gitupdate",
                url: "git/update",
                defaults: new { controller = "Markdown", action = "UpdateRepo" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
        }
    }
}