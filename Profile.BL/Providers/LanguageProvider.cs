using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using log4net;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.BL.Providers
{
    public class LanguageProvider : ILanguageProvider
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private IProfileContext _context;

        public LanguageProvider(IProfileContext profileContext)
        {
            _context = profileContext;
        }

        public List<Language> GetLanguagesByProfileId(int profileId)
        {
            var languages = _context.Languages.Where(l => l.TraineeProfileId == profileId).ToList();
            return languages;
        }

        public List<Language> UpdateLanguages(List<Language> languages)
        {
            foreach (var language in languages)
            {
                if (language.Id == 0)
                {
                    _context.Languages.Add(language);
                }
                else
                {
                    _context.Entry(language).State = EntityState.Modified;
                }
            }

            _context.SaveChanges();

            return languages;
        }

        public void RemoveLanguage(int languageId)
        {
            if (languageId > 0)
            {
                Language language = new Language { Id = languageId };

                _context.Entry(language).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            else
            {
                Logger.Error($"RemoveLanguage: not found language with Id={languageId}");
            }
        }
    }
}
