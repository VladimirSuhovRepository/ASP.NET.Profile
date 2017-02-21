using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Json
{
    [JsonObject]
    [JsonNetModel]
    public class ResetPasswordResponseJsonModel
    {
        public bool Succeeded { get; set; }
        public string Error { get; set; }
    }
}