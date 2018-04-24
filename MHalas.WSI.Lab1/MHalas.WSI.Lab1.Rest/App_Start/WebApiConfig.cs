using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace MHalas.WSI.Lab1.Rest
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(Properties.Settings.Default.MediaTypeHeaderValue));

            GlobalConfiguration.Configuration.Formatters.Clear();

            if(Properties.Settings.Default.SupportJson)
                GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());
            if (Properties.Settings.Default.SupportXML)
                GlobalConfiguration.Configuration.Formatters.Add(new XmlMediaTypeFormatter());
        }
    }
}
