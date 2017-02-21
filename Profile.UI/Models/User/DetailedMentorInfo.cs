namespace Profile.UI.Models.User
{
    public class DetailedMentorInfo : MentorInfo, IDetailedInfo
    {
        private const string ViewName = "_MentorDetails";

        public string PartialViewName => ViewName;
    }
}