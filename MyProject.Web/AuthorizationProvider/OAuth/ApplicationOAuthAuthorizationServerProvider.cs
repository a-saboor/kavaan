using MyProject.Service;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyProject.Web.AuthorizationProvider.OAuth
{
    public class ApplicationOAuthAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            await Task.Run(() =>
            {
                context.Validated();
            });
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            string Message = string.Empty;
            try
            {
                var deviceId = (await context.Request.ReadFormAsync())["deviceId"];
                var type = (await context.Request.ReadFormAsync())["type"];
                var countryCode = (await context.Request.ReadFormAsync())["CountryCode"];

                if (String.IsNullOrEmpty(deviceId))
                {
                    context.SetError("Invalid Access", "Device Id required.");
                    return;
                }

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                if (type == "Customer")
                {
                    bool verifyByEmail = false;
                    bool verifyByContact = false;
                    bool verifyAgain = false;

                    if (context.UserName.StartsWith("971") || context.UserName.StartsWith("+971"))//In dubai, contact verify by otp verification
                        verifyByContact = true;
                    else
                        verifyByEmail = true;


                    var _customerService = DependencyResolver.Current.GetService<ICustomerService>();

                    if (_customerService.Authenticate(countryCode, context.UserName, context.Password, ref identity, ref Message , ref verifyByEmail ,ref  verifyByContact , ref verifyAgain))
                    {
                        //roles example
                        var rolesTechnicalNamesUser = new string[] { "Branch" };

                        var principal = new GenericPrincipal(identity, rolesTechnicalNamesUser);
                        Thread.CurrentPrincipal = principal;

                        context.Validated(identity);
                    }
                    else
                    {
                        context.SetError("Invalid Access", Message);
                        return;
                    }
                }

                else if (type == "Vendor")

                {
                    var _vendorUserService = DependencyResolver.Current.GetService<IVendorUserService>();

                    var user= countryCode + context.UserName;

                    if (_vendorUserService.Authenticate(user, context.Password, ref identity, ref Message))
                    {

                        //var _coachSessionService = DependencyResolver.Current.GetService<ICoachSessionService>();
                        //_coachSessionService.ExpireSession(0, deviceId, "Coach");

                        //roles example
                        var rolesTechnicalNamesUser = new string[] { "Branch" };

                        var principal = new GenericPrincipal(identity, rolesTechnicalNamesUser);
                        Thread.CurrentPrincipal = principal;

                        context.Validated(identity);
                    }
                    else
                    {
                        context.SetError("Invalid Access", Message);
                        return;
                    }
                }
                else if (type == "Technician")

                {
                    var _staffService = DependencyResolver.Current.GetService<IStaffService>();

                    //var user = countryCode + context.UserName;

                    if (_staffService.Authenticate(context.UserName, context.Password, ref identity, ref Message, countryCode))
                    {

                        //roles example
                        var rolesTechnicalNamesUser = new string[] { "Branch" };

                        var principal = new GenericPrincipal(identity, rolesTechnicalNamesUser);
                        Thread.CurrentPrincipal = principal;

                        context.Validated(identity);
                    }
                    else
                    {
                        context.SetError("Invalid Access", Message);
                        return;
                    }
                }
                else
                {
                    context.SetError("Invalid Access", "Unknown type.");
                    return;
                }
            }
            catch (Exception ex)
            {
                //Logs.Write(ex);
                context.SetError("Invalid Access", "Internal server error.");
            }
        }
    }
}