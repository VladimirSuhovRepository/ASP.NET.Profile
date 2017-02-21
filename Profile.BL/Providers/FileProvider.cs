using System.Data.Entity;
using System.Linq;
using Profile.BL.Interfaces;
using Profile.DAL.Context;
using Profile.DAL.Entities;

namespace Profile.BL.Providers
{
    public class FileProvider : IFileProvider
    {
        private IProfileContext _context;

        public FileProvider(IProfileContext profileContext)
        {
            _context = profileContext;
        }

        public File GetFile(int id)
        {
            return _context.Files.Find(id);
        }

        public File GetFileWithData(int id)
        {
            return _context.Files.Where(f => f.Id == id).Include(f => f.FileData).Single();
        }

        public File AddFile(File file)
        {
            _context.Files.Add(file);
            _context.SaveChanges();

            return file;
        }

        public File UpdateFile(File file)
        {
            var fileDataModel = file.FileData.Data == null 
                ? GetFile(file.Id)
                : GetFileWithData(file.Id);

            fileDataModel.Description = file.Description;

            if (file.FileData.Data != null)
            {
                fileDataModel.Name = file.Name;
                fileDataModel.Size = file.Size;
                fileDataModel.FileData.Data = file.FileData.Data;
            }

            _context.SaveChanges();

            return fileDataModel;
        }

        public void RemoveFile(int id)
        {
            var file = new File { Id = id };

            var entry = _context.Entry(file);
            entry.State = EntityState.Deleted;

            _context.SaveChanges();
        }
    }
}
