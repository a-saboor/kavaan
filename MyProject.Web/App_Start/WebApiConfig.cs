﻿using Microsoft.Owin.Security.OAuth;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Project.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services  
            // Configure Web API to use only bearer token authentication.  
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/v1/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        //    config.Routes.MapHttpRoute(
        //    name: "VendorApi",
        //    routeTemplate: "api/v1/vendor/{controller}/{id}",
        //    defaults: new { id = RouteParameter.Optional }
        //);
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));

            // WebAPI when dealing with JSON & JavaScript!  
            // Setup json serialization to serialize classes to camel (std. Json format)  
            var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();

            // Adding JSON type web api formatting.  
            config.Formatters.Clear();
            config.Formatters.Add(formatter);
        }
    }
}
