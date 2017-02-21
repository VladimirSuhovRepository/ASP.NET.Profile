using System;
using System.Collections.Generic;
using AutoMapper;
using Profile.DAL.Entities;
using Profile.UI.ModelEnums;

namespace Profile.UI.Mappers
{
    public class RoleMapper
    {
        private IMapper _mapper;
        private Dictionary<UIRoleType, List<RoleType>> _domainRolesFromUI;
        public RoleMapper(IMapper mapper)
        {
            _mapper = mapper;
            _domainRolesFromUI = new Dictionary<UIRoleType, List<RoleType>>
            {
                { UIRoleType.Trainee, new List<RoleType> { RoleType.Trainee } },
                { UIRoleType.Mentor, new List<RoleType> { RoleType.Mentor } },
                { UIRoleType.HR, new List<RoleType> { RoleType.HR } },
                { UIRoleType.ScrumMaster, new List<RoleType> { RoleType.ScrumMaster } },
                { UIRoleType.MentorScrumMaster, new List<RoleType> { RoleType.ScrumMaster, RoleType.Mentor } },
            };
        }

        public List<RoleType> ToDomainRoleTypes(UIRoleType roleUI)
        {
            return _domainRolesFromUI[roleUI];
        }
    }
}