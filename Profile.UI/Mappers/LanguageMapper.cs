using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Profile.DAL.Entities;
using Profile.UI.Models.Json;
using Profile.UI.Models.Profile;

namespace Profile.UI.Mappers
{
    public class LanguageMapper
    {
        private IMapper _mapper;

        public LanguageMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Language FromViewModel(int profileId, LanguageViewModel languageViewModel)
        {
            var language = _mapper.Map<LanguageViewModel, Language>(languageViewModel);

            language.TraineeProfileId = profileId;

            return language;
        }

        public LanguageViewModel ToViewModel(Language language)
        {
            var languageViewModel = _mapper.Map<Language, LanguageViewModel>(language);

            return languageViewModel;
        }

        public List<Language> FromProfileLanguagesJsonModel(int profileId, ProfileLanguagesJson profileLanguagesJson)
        {
            List<Language> languages = profileLanguagesJson.Languages
                .Select(language => FromViewModel(profileId, language))
                .ToList();

            return languages;
        }

        public ProfileLanguagesJson ToProfileLanguagesJsonModel(int profileId, List<Language> languages)
        {
            ProfileLanguagesJson profileLanguagesJson = new ProfileLanguagesJson
            {
                ProfileId = profileId,
                Languages = languages.Select(ToViewModel).ToList()
            };

            return profileLanguagesJson;
        }
    }
}