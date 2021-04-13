using System.Web;
using System.Web.Mvc;

namespace Ely_Yohanna_Vega
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
