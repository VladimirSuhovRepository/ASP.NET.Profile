using Newtonsoft.Json;
using Profile.UI.Attributes;
using Profile.UI.ModelEnums;

namespace Profile.UI.Models.Manager
{
    [JsonObject]
    [JsonNetModel]
    public class NewUsersToRolesJsonModel
    {
        [JsonProperty("Users")]
        public int[] UserIds { get; set; }
        public UIRoleType Role { get; set; }
        [JsonProperty("Spec")]
        public int? SpecializationId { get; set; }
    }
}