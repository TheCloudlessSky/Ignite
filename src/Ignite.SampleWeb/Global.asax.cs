using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ignite.SampleWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RegisterPackages(routes);

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        private static void RegisterPackages(RouteCollection routes)
        {
            var container = Ignite.Create("assets")
                .DisableDebugging()
#if DEBUG
                .DisableCaching();
#else
                .EnableCaching();
#endif

            container
                .JavaScript("core", new[]
                { 
                    "scripts/vendor/underscore*.js",
                    "scripts/vendor/backbone*.js",
                    "scripts/a/*.js",
                    "scripts/b/*.js",
                    "scripts/templates/*.jst"
                });

            container
                .StyleSheet("core", new[]
                {
                    "content/**/*.less"
                });
            //    .StyleSheet("ie", new[]
            //    {
            //        "content/style/ie/*.less"
            //    });

            container.Build(routes);
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}