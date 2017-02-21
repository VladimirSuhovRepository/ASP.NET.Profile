using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface ILinkProvider
    {
        Link GetLink(int id);
        Link AddLink(Link file);
        Link UpdateLink(Link file);
        void RemoveLink(int id);
    }
}
