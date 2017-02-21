using System.Collections.Generic;

namespace Profile.UI.Models.User
{
    public class UserProfile
    {
        public UserProfile()
        {
            Details = new List<IDetailedInfo>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public UserContacts Contacts { get; set; }
        public IList<IDetailedInfo> Details { get; set; }
        public bool IsOwner { get; set; }

        public bool HasEmail => !string.IsNullOrEmpty(Contacts?.Email);
        public bool HasPhone => !string.IsNullOrEmpty(Contacts?.Phone);
        public bool HasSkype => !string.IsNullOrEmpty(Contacts?.Skype);
        public bool HasLinkedIn => !string.IsNullOrEmpty(Contacts?.LinkedIn);
    }
}