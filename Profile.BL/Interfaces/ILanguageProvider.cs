using System;
using System.Collections.Generic;
using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface ILanguageProvider
    {
        List<Language> GetLanguagesByProfileId(int profileId);
        List<Language> UpdateLanguages(List<Language> languages);
        void RemoveLanguage(int id);
    }
}
