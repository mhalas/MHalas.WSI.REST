using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MHalas.WSI.REST.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();


            var cors = new EnableCorsAttribute("*", "*", "*", "*");
            config.EnableCors(cors);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(Properties.Settings.Default.MediaTypeHeaderValue));

            GlobalConfiguration.Configuration.Formatters.Clear();

            GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());
            GlobalConfiguration.Configuration.Formatters.Add(new XmlMediaTypeFormatter());
            //if (Properties.Settings.Default.SupportJson)
            //if (Properties.Settings.Default.SupportXML)
        }
    }
}
