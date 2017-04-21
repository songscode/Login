using System.Web;
using System.Web.Mvc;
using Login.Web.Code;

namespace Login.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new FormAuthorizeAttribute());
        }
    }
}
