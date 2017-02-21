namespace Profile.UI.Models.User
{
    public class DetailedScrumMasterInfo : ScrumMasterInfo, IDetailedInfo
    {
        private const string ViewName = "_ScrumMasterDetails";

        public string PartialViewName => ViewName;
    }
}