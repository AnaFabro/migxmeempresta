using Migx.Web.Providers;
using System.Web;
using System.Web.Mvc;

namespace Migx.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthorizeSimples());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
