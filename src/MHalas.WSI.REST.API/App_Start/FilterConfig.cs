using System.Web;
using System.Web.Mvc;

namespace MHalas.WSI.REST.API
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
