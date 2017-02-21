using System.Collections.Generic;
using AutoMapper;
using Profile.DAL.Entities;
using Profile.UI.Models.User;
using System.Linq;
using AutoMapper;
using Profile.DAL.Entities;
using Profile.UI.Models;
using Profile.UI.Models.Manager;
using Profile.UI.Utility;

namespace Profile.UI.Mappers
{
    public class UserMapper
    {
        private IMapper _mapper;

        public UserMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public UserProfile ToUserProfile(User user,
                                         IEnumerable<IDetailedInfo> userDetails,
                                         bool isOwner)
        {
            var userProfile = _mapper.Map<UserProfile>(user);

            foreach (var details in userDetails)
            {
                userProfile.Details.Add(details);
            }

            userProfile.IsOwner = isOwner;

            return userProfile;
        }

        public NewUsersSetRolesViewModel ToNewUsersSetRolesViewModel(List<User> users, 
                                                                     List<Specialization> specializations)
        {
            var sortedSpecializations = specializations.OrderBy(s => s, new SpecializationComparer()).ToList();
            var viewModel = new NewUsersSetRolesViewModel
            {
                Users = users.Select(_mapper.Map<User, NewUserViewModel>).OrderBy(u => u.FullName).ToList(),
                Specializations = sortedSpecializations.Select(_mapper.Map<Specialization, SpecializationNameViewModel>)
                                                       .OrderBy(u => u.Name).ToList()
            };

            return viewModel;
        }
    }
}