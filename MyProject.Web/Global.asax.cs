using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Linq;

namespace MyProject.Web
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			//GlobalConfiguration.Configure(WebApiConfig.Register);
			//AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			log4net.Config.XmlConfigurator.Configure();

			// Autofac and Automapper configurations
			Bootstrapper.Run();
		}

		protected void Application_EndRequest()
		{
		}

	}

}
