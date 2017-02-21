using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Json
{
    [JsonObject]
    [JsonNetModel]
    public class ResetPasswordRequestJsonModel
    {
        [JsonProperty("pass1")]
        public string Password { get; set; }
        [JsonProperty("pass2")]
        public string ConfirmPassword { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}