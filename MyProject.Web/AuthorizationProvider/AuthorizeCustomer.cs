using MyProject.Service;
using MyProject.Web.Helpers;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyProject.Web.AuthorizationProvider
{
	public class AuthorizeCustomer : AuthorizeAttribute
	{
		public string AccessLevel { get; set; }

		//private readonly ICustomerService _customerService;
		private ICustomerService _customerService
		{
			get
			{
				return DependencyResolver.Current.GetService<ICustomerService>();
			}
		}

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			var AccessToken = httpContext.Request.Cookies["Customer-Session"] != null ? httpContext.Request.Cookies["Customer-Session"]["Access-Token"] : null;

			if (httpContext.Session["CustomerID"] != null && httpContext.Session["CustomerName"] != null)
			{
				return true;
			}
            else if (AccessToken != null && !string.IsNullOrEmpty(AccessToken))
            {
                var customer = _customerService.GetByAuthCode(AccessToken);
                if (customer != null)
                {
                    httpContext.Session["CustomerID"] = customer.ID;
                    httpContext.Session["CustomerName"] = customer.UserName;
                    httpContext.Session["Contact"] = customer.Contact;
                    httpContext.Session["Email"] = customer.Email;
                    httpContext.Session["Photo"] = customer.Logo;
                    httpContext.Session["Points"] = customer.Points;
                    httpContext.Session["AccountType"] = customer.AccountType;
                    httpContext.Session["ReceiverType"] = "Customer";

					httpContext.Response.Cookies["Customer-Details"]["Name"] = CustomerSessionHelper.UserName;
					httpContext.Response.Cookies["Customer-Details"]["Logo"] = CustomerSessionHelper.Photo;
					httpContext.Response.Cookies["Customer-Session"]["Access-Token"] = customer.AuthorizationCode;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			var returnUrl = string.Empty;
			try
			{
				if (filterContext.HttpContext.Request.Url.AbsolutePath.Contains("/en-ae"))
				{
					returnUrl = filterContext.HttpContext.Request.Url.AbsolutePath.Replace("/en-ae", "");
				}
				else
				{
					returnUrl = filterContext.HttpContext.Request.Url.AbsolutePath.Replace("/ar-ae", "");
				}
			}
			catch (System.Exception)
			{ }

			if (filterContext.HttpContext.Request.IsAjaxRequest())
			{
				var urlHelper = new UrlHelper(filterContext.RequestContext);
				filterContext.HttpContext.Response.StatusCode = 403;
				filterContext.Result = new JsonResult
				{
					Data = new
					{
						Error = "NotAuthorized",
						//LogOnUrl = urlHelper.Action("Login", "Account", new { area = "CustomerPortal" })
						LogOnUrl = urlHelper.Action("Index", "Home", new { area = "", returnUrl = returnUrl })
					},
					JsonRequestBehavior = JsonRequestBehavior.AllowGet
				};
			}
			else
			{
				filterContext.Result = new RedirectToRouteResult(
					//new RouteValueDictionary(new { controller = "Account", action = "Login", area = "CustomerPortal" })
					new RouteValueDictionary(new { controller = "Home", action = "Index", area = "", returnUrl = returnUrl })
					);
			}

		}
	}
}