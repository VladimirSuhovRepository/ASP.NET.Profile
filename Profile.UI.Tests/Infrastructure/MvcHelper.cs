using System;
using System.Collections;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace Profile.UI.Tests.Infrastructure
{
    public static class MvcHelper
    {
        public const string AppPathModifier = "/$(SESSION)";

        public static HtmlHelper<object> GetHtmlHelper()
        {
            return GetHtmlHelper("Home", "Index");
        }

        public static HtmlHelper<object> GetHtmlHelper(string controllerName,
                                                       string actionName)
        {
            return GetHtmlHelper(controllerName, actionName, null);
        }

        public static HtmlHelper<object> GetHtmlHelper(string controllerName, 
                                                       string actionName,
                                                       object routeValues)
        {
            RouteCollection rt = new RouteCollection();

            rt.Add(new Route("{controller}/{action}/{id}", null)
            {
                Defaults = new RouteValueDictionary(new { id = "defaultid" })
            });

            var routes = new RouteValueDictionary(routeValues);

            routes.Add("controller", controllerName);
            routes.Add("action", actionName);

            RouteData rd = new RouteData();

            foreach (var route in routes)
            {
                rd.Values.Add(route.Key, route.Value.ToString());
            }

            HttpContextBase httpcontext = GetHttpContext("/app/", null, null, rd);
            ViewDataDictionary vdd = new ViewDataDictionary();

            ViewContext viewContext = new ViewContext()
            {
                HttpContext = httpcontext,
                RouteData = rd,
                ViewData = vdd
            };

            Mock<IViewDataContainer> mockVdc = new Mock<IViewDataContainer>();

            mockVdc.Setup(vdc => vdc.ViewData).Returns(vdd);

            HtmlHelper<object> htmlHelper = new HtmlHelper<object>(viewContext, mockVdc.Object, rt);

            return htmlHelper;
        }

        public static HttpContextBase GetHttpContext(string appPath, 
                                                     string requestPath, 
                                                     string httpMethod, 
                                                     string protocol, 
                                                     int port,
                                                     RouteData routeData)
        {
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();

            if (!string.IsNullOrEmpty(appPath))
            {
                mockHttpContext.Setup(o => o.Request.ApplicationPath).Returns(appPath);
                mockHttpContext.Setup(o => o.Request.RawUrl).Returns(appPath);
            }

            if (!string.IsNullOrEmpty(requestPath))
            {
                mockHttpContext.Setup(o => o.Request.AppRelativeCurrentExecutionFilePath).Returns(requestPath);
            }

            Uri uri;

            if (port >= 0)
            {
                uri = new Uri($"{protocol}://localhost:{port}");
            }
            else
            {
                uri = new Uri($"{protocol}://localhost");
            }

            mockHttpContext.Setup(o => o.Request.Url).Returns(uri);
            mockHttpContext.Setup(o => o.Request.PathInfo).Returns(string.Empty);

            if (!string.IsNullOrEmpty(httpMethod))
            {
                mockHttpContext.Setup(o => o.Request.HttpMethod).Returns(httpMethod);
            }

            mockHttpContext.Setup(o => o.Session).Returns((HttpSessionStateBase)null);
            mockHttpContext.Setup(o => o.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(r => AppPathModifier + r);
            mockHttpContext.Setup(o => o.Items).Returns(new Hashtable());

            if (routeData != null)
            {
                var httpRequest = GetHttpRequest(mockHttpContext.Object, routeData);

                mockHttpContext.Setup(o => o.Request).Returns(httpRequest);
            }

            return mockHttpContext.Object;
        }

        public static HttpContextBase GetHttpContext(string appPath, 
                                                     string requestPath, 
                                                     string httpMethod, 
                                                     RouteData routeData)
        {
            return GetHttpContext(appPath, requestPath, httpMethod, Uri.UriSchemeHttp.ToString(), -1, routeData);
        }

        public static ControllerContext GetControllerContext(string currentAction,
                                                             string currentController,
                                                             ControllerBase controller)
        {
            var routeData = new RouteData();

            routeData.Values.Add("controller", currentController);
            routeData.Values.Add("action", currentAction);

            var httpContext = GetHttpContext(null, null, null, routeData);

            return new ControllerContext(httpContext, routeData, controller);
        }

        public static HttpRequestBase GetHttpRequest(HttpContextBase httpContext, RouteData routeData)
        {
            var mockHttpRequest = new Mock<HttpRequestBase>();
            var requestContext = new RequestContext(httpContext, routeData);

            mockHttpRequest.Setup(o => o.RequestContext).Returns(requestContext);

            return mockHttpRequest.Object;
        }
    }
}