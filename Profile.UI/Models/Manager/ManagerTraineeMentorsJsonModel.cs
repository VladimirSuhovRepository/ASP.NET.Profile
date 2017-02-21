using System.Collections.Generic;
using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Manager
{
    [JsonObject]
    [JsonNetModel]
    public class ManagerTraineeMentorsJsonModel
    {
        public ManagerTraineeMentorsJsonModel()
        {
            Trainees = new List<UserNameCommonModel>();
            Mentors = new List<UserNameCommonModel>();
        }

        public List<UserNameCommonModel> Trainees { get; set; }
        public List<UserNameCommonModel> Mentors { get; set; }
    }
}