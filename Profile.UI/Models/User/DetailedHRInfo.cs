namespace Profile.UI.Models.User
{
    public class DetailedHRInfo : HRInfo, IDetailedInfo
    {
        private readonly string _partialViewName;

        public DetailedHRInfo(string partialViewName)
        {
            _partialViewName = partialViewName;
        }

        public string PartialViewName => _partialViewName;
    }
}