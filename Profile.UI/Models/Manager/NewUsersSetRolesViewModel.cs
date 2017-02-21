using System.Collections.Generic;

namespace Profile.UI.Models.Manager
{
    public class NewUsersSetRolesViewModel
    {
        public NewUsersSetRolesViewModel()
        {
            Users = new List<NewUserViewModel>();
            Specializations = new List<SpecializationNameViewModel>();
        }

        public List<NewUserViewModel> Users { get; set; }
        public List<SpecializationNameViewModel> Specializations { get; set; }
    }
}