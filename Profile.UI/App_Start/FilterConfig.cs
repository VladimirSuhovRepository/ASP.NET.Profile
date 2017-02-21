using System.Web.Mvc;
using Profile.UI.Filters;

namespace Profile.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new JsonNetActionFilter());
            filters.Add(new PermissionAuthorizeAttribute());
        }
    }
}
