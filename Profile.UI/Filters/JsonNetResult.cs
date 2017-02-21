using System;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Profile.UI.Filters
{
    public class JsonNetResult : JsonResult
    {
        public JsonNetResult()
        {
            Settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Error
            };
        }

        public JsonSerializerSettings Settings { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            HttpResponseBase response = context.HttpContext.Response;

            if (ContentEncoding != null) response.ContentEncoding = ContentEncoding;

            if (Data == null) return;

            response.ContentType = string.IsNullOrEmpty(ContentType) ? "application/json" : ContentType;

            var scriptSerializer = JsonSerializer.Create(Settings);

            // Serialize the data to the Output stream of the response
            scriptSerializer.Serialize(response.Output, this.Data);
        }
    }
}
