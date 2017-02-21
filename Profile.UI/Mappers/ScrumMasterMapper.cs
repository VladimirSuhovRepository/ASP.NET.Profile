using AutoMapper;
using Profile.DAL.Entities;
using Profile.UI.Models.User;

namespace Profile.UI.Mappers
{
    public class ScrumMasterMapper
    {
        private IMapper _mapper;

        public ScrumMasterMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public DetailedScrumMasterInfo ToDetailedScrumMasterInfo(ScrumMaster scrumMaster)
        {
            return _mapper.Map<DetailedScrumMasterInfo>(scrumMaster);
        }
    }
}