using System.Web.Mvc;

namespace MyProject.Web.Areas.CustomerPortal
{
	public class CustomerPortalAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get
			{
				return "CustomerPortal";
			}
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
            context.MapRoute(
                "CustomerPortal_default",
                "Customer/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            //context.MapRoute(
            //	"CustomerPortal_default",
            //	"customer/{controller}/{action}/{id}",
            //	new { action = "Index", id = UrlParameter.Optional },
            //	namespaces: new[] { "MyProject.Web.Areas.CustomerPortal.Controllers" }
            //);
        }
	}
}