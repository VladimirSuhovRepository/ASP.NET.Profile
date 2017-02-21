using System.Data.Entity;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.BL.Providers
{
    public class LinkProvider : ILinkProvider
    {
        private IProfileContext _context;

        public LinkProvider(IProfileContext profileContext)
        {
            _context = profileContext;
        }

        public Link GetLink(int id)
        {
            return _context.Links.Find(id);
        }

        public Link AddLink(Link link)
        {
            _context.Links.Add(link);
            _context.SaveChanges();

            return link;
        }

        public Link UpdateLink(Link link)
        {
            var entry = _context.Entry(link);
            entry.State = EntityState.Modified;

            _context.SaveChanges();

            return link;
        }

        public void RemoveLink(int id)
        {
            var link = new Link { Id = id };

            var entry = _context.Entry(link);
            entry.State = EntityState.Deleted;

            _context.SaveChanges();
        }
    }
}
