using MyProject.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyProject.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //enabling attribute routing
            routes.MapMvcAttributeRoutes();
            AreaRegistration.RegisterAllAreas();

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //    namespaces: new[] { "MyProject.Web.Controllers" }
            //).DataTokens.Add("area", "");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "MyProject.Web.Controllers" }
            ).DataTokens.Add("area", "");


            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
            //    namespaces: new[] { "MyProject.Web.Areas.Admin.Controllers" })
            //    .DataTokens.Add("area", "Admin");
            
            //routes.MapRoute(
            //  name: "AdminLogin",
            //  url: "admin",
            //  defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
            //  namespaces: new[] { "MyProject.Web.Areas.Admin.Controllers" })
            //  .DataTokens.Add("area", "Admin");

            //routes.MapRoute(
            //	name: "VendorLogin",
            //	url: "Vendor",
            //	defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
            //	namespaces: new[] { "MyProject.Web.Areas.VendorPortal.Controllers" })
            //	.DataTokens.Add("area", "VendorPortal");

        }
    }
}
