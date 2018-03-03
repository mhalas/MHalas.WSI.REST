using System.Web;
using System.Web.Mvc;

namespace MHalas.WSI.Lab1.Rest
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
