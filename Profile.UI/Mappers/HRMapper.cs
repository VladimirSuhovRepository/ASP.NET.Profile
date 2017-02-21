using AutoMapper;
using Profile.DAL.Entities;
using Profile.UI.Models.User;

namespace Profile.UI.Mappers
{
    public class HRMapper
    {
        private IMapper _mapper;

        public HRMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public DetailedHRInfo ToDetailedHRInfo(Contacts contacts, string partialViewName)
        {
            var hrInfo = new DetailedHRInfo(partialViewName);

            return _mapper.Map(contacts, hrInfo);
        }
    }
}