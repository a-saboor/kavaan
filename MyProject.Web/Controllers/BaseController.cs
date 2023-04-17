using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using MyProject.Web.Helpers;


namespace MyProject.Web.Controllers
{
    public class BaseController : Controller
    {
        //GET: Base => depends on route value
        protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        {
            string cultureName = null;
            HttpCookie cookie = Request.Cookies["_culture"];

            if (Request.RawUrl.Contains("page-not-found") && !string.IsNullOrEmpty(cookie.Value))
            {
                cultureName = cookie.Value;
            }
            else
            {
                if (Request.RawUrl.Length > 2) // Attempt to ignore the route issue
                {
                    cultureName = RouteData.Values["culture"] as string;
                }
                if (cultureName == null || (!cultureName.Contains("en") && !cultureName.Contains("ar")))
                {
                    if (Request.RawUrl.Contains("/en-us/"))
                        cultureName = "en-ae";
                    else if (Request.RawUrl.Contains("/ar-us/"))
                        cultureName = "ar-ae";
                    else if (Request.RawUrl.Contains("/en"))
                        cultureName = "en-ae";
                    else if (Request.RawUrl.Contains("/ar"))
                        cultureName = "ar-ae";
                    else
                    {
                        // Attempt to read the culture cookie from Request
                        HttpCookie cultureCookie = Request.Cookies["_culture"];
                        if (cultureCookie != null)
                            cultureName = cultureCookie.Value;
                    }
                    if (cultureName == null)
                    {
                        // obtain it from HTTP header AcceptLanguages
                        cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ? Request.UserLanguages[0] : null;
                    }
                }

                // Validate culture name
                cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe
            }

            #region comment
            //if (RouteData.Values["culture"] as string != cultureName)
            //{

            //	// Force a valid culture in the URL
            //	RouteData.Values["culture"] = cultureName.ToLowerInvariant(); // lower case too

            //	// Redirect user
            //	Response.RedirectToRoute(RouteData.Values);
            //}
            #endregion

            #region Cookie value change if lang change
            if (cookie == null)
            {
#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
                cookie = new HttpCookie("_culture");
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
                cookie.Value = cultureName;
#pragma warning disable SecurityIntelliSenseCS // MS Security rules violation
                cookie.Expires = DateTime.Now.AddYears(1);
#pragma warning restore SecurityIntelliSenseCS // MS Security rules violation
                Response.Cookies.Add(cookie);
            }
            else
            {
                if (cookie.Value != cultureName)
                {
                    cookie.Value = cultureName;
                    Response.Cookies.Add(cookie);
                }
            }
            #endregion

            // Modify current thread's cultures            
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            return base.BeginExecuteCore(callback, state);
        }

        #region Cookie based
        //// GET: Base => depends on cookie value
        //protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
        //{
        //	string cultureName = null;

        //	// Attempt to read the culture cookie from Request
        //	HttpCookie cultureCookie = Request.Cookies["_culture"];
        //	if (cultureCookie != null)
        //		cultureName = cultureCookie.Value;
        //	else
        //		cultureName = Request.UserLanguages != null && Request.UserLanguages.Length > 0 ?
        //				Request.UserLanguages[0] :  // obtain it from HTTP header AcceptLanguages
        //				null;
        //	// Validate culture name
        //	cultureName = CultureHelper.GetImplementedCulture(cultureName); // This is safe

        //	// Modify current thread's cultures            
        //	Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
        //	Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

        //	return base.BeginExecuteCore(callback, state);
        //}
        #endregion

    }
}