using System.Web.Mvc;
using Profile.BL.Interfaces;
using Profile.DAL.Entities;
using Profile.UI.Extentions;
using Profile.UI.Models.Json;

namespace Profile.UI.Controllers
{
    public class FileController : Controller
    {
        private IFileProvider _fileProvider;

        public FileController(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        [HttpGet]
        public FileResult Download(int id)
        {
            var file = _fileProvider.GetFileWithData(id);
            return File(file.FileData.Data, System.Net.Mime.MediaTypeNames.Application.Octet, file.Name);
        }

        [HttpPost]
        public JsonResult Add(FileJsonRequestModel fileJson)
        {
            var uploadingFile = new File
            {
                TraineeProfileId = fileJson.ProfileId,
                Name = fileJson.File?.FileName,
                Size = fileJson.File?.ContentLength ?? 0,
                Description = fileJson.Description.TrimAndUppercaseFirst(),
                FileData = new FileData
                {
                    Id = fileJson.Id,
                    Data = fileJson.GetFileData()
                }
            };

            var addedFile = _fileProvider.AddFile(uploadingFile);
            var addedFileJsonModel = new FileJsonResponseModel
            {
                Id = addedFile.Id,
                Name = addedFile.Name,
                Description = addedFile.Description,
                Size = addedFile.Size
            };

            return Json(addedFileJsonModel);
        }

        [HttpPut]
        public JsonResult Update(FileJsonRequestModel fileJson)
        {
            var updatingFile = new File
            {
                Id = fileJson.Id,
                Name = fileJson.File?.FileName,
                Size = fileJson.File?.ContentLength ?? 0,
                Description = fileJson.Description.TrimAndUppercaseFirst(),
                FileData = new FileData
                {
                    Id = fileJson.Id,
                    Data = fileJson.GetFileData()
                }
            };

            var updatedFile = _fileProvider.UpdateFile(updatingFile);

            var updatedFileJsonModel = new FileJsonResponseModel
            {
                Id = updatedFile.Id,
                Name = updatedFile.Name,
                Description = updatedFile.Description,
                Size = updatedFile.Size
            };

            return Json(updatedFileJsonModel);
        }

        [HttpDelete]
        public void Remove(int id)
        {
            _fileProvider.RemoveFile(id);
        }
    }
}