namespace Profile.UI.Models.User
{
    public class DetailedManagerInfo : IDetailedInfo
    {
        private const string ViewName = "_ManagerDetails";

        public string PartialViewName => ViewName;
    }
}