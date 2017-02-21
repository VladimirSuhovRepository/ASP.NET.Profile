using Newtonsoft.Json;
using Profile.UI.Attributes;
using Profile.UI.ModelEnums;

namespace Profile.UI.Models.Manager
{
    [JsonObject]
    [JsonNetModel]
    public class NewUsersForRemovingJsonModel
    {
        [JsonProperty("Users")]
        public int[] UserIds { get; set; }
    }
}