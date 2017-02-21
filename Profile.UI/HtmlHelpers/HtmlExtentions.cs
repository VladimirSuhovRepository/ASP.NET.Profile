using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Profile.UI.HtmlHelpers
{
    public static class HtmlExtentions
    {
        public static bool IsCurrentRoute(this HtmlHelper helper,
                                          string controller,
                                          string action)
        {
            return helper.IsCurrentRoute(controller, action, null);
        }

        /// <summary>
        /// Determines whether current route matches with entered parameters.
        /// </summary>
        /// <param name="helper">Instance of extended class.</param>
        /// <param name="controller">Controller name.</param>
        /// <param name="action">Action name.</param>
        /// <param name="routeValues">Values are based on properties from the specified object.</param>
        /// <returns>Bool value that indicates is the current route matches input parameters.</returns>
        public static bool IsCurrentRoute(this HtmlHelper helper, 
                                          string controller, 
                                          string action,
                                          object routeValues)
        {
            var routes = new RouteValueDictionary(routeValues);

            routes.Add("controller", controller);
            routes.Add("action", action);

            return helper.IsCurrentRoute(routes);
        }

        public static bool IsCurrentRoute(this HtmlHelper helper, RouteValueDictionary routeValues)
        {
            var currentRouteValues = helper.ViewContext.HttpContext.Request.RequestContext.RouteData.Values;

            if (currentRouteValues.Count != routeValues.Count) return false;

            bool isCurrentRoute = true;

            foreach (var routeKey in routeValues.Keys)
            {
                object currentValue;

                if (currentRouteValues.TryGetValue(routeKey, out currentValue))
                {
                    isCurrentRoute &= currentValue.Equals(routeValues[routeKey].ToString());
                }
            }

            return isCurrentRoute;
        }

        public static HtmlString MenuItem(this HtmlHelper htmlHelper,
                                          string linkText,
                                          string actionName,
                                          string controllerName)
        {
            return htmlHelper.MenuItem(linkText, actionName, controllerName, null, null);
        }

        public static HtmlString MenuItem(this HtmlHelper htmlHelper,
                                          string linkText,
                                          string actionName,
                                          string controllerName,
                                          object routeValues,
                                          object linkHtmlAttributes)
        {
            var stringBuilder = new StringBuilder();

            var actionLinkHtml = htmlHelper.ActionLink(linkText,
                                                       actionName,
                                                       controllerName,
                                                       routeValues,
                                                       linkHtmlAttributes);

            string itemClass = htmlHelper.IsCurrentRoute(controllerName, actionName, routeValues) ?
                "active" :
                string.Empty;

            stringBuilder.Append($"<li class='{itemClass}'>");
            stringBuilder.Append(actionLinkHtml.ToHtmlString());
            stringBuilder.Append("</li>");

            return new HtmlString(stringBuilder.ToString());
        }

        public static HtmlString MenuItem(this HtmlHelper htmlHelper,
                                          string linkText,
                                          string actionName,
                                          string controllerName,
                                          bool isEnabled)
        {
            return htmlHelper.MenuItem(linkText,
                                       actionName,
                                       controllerName,
                                       null,
                                       null,
                                       isEnabled);
        }

        public static HtmlString MenuItem(this HtmlHelper htmlHelper,
                                          string linkText,
                                          string actionName,
                                          string controllerName,
                                          object routeValues,
                                          object linkHtmlAttributes,
                                          bool isEnabled)
        {
            if (isEnabled)
            {
                return htmlHelper.MenuItem(linkText,
                                           actionName,
                                           controllerName,
                                           routeValues,
                                           linkHtmlAttributes);
            }

            return new HtmlString($@"<li class=""disabled""><a>{linkText}</a></li>");
        }
    }
}