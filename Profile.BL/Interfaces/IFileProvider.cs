using Profile.DAL.Entities;

namespace Profile.BL.Interfaces
{
    public interface IFileProvider
    {
        File GetFile(int id);
        File GetFileWithData(int id);
        File AddFile(File file);
        File UpdateFile(File file);
        void RemoveFile(int id);
    }
}
