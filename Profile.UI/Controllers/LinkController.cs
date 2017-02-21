using System.Web.Mvc;
using Profile.BL.Interfaces;
using Profile.DAL.Entities;
using Profile.UI.Extentions;
using Profile.UI.Models.Json;

namespace Profile.UI.Controllers
{
    public class LinkController : Controller
    {
        private ILinkProvider _linkProvider;

        public LinkController(ILinkProvider linkProvider)
        {
            _linkProvider = linkProvider;
        }

        [HttpPost]
        public JsonResult Add(LinkJsonRequestModel linkJson)
        {
            var addingLink = new Link
            {
                Id = linkJson.Id,
                Url = linkJson.Url.Trim(),
                Description = linkJson.Description.TrimAndUppercaseFirst(),
                TraineeProfileId = linkJson.TraineeProfileId
            };

            var addedLink = _linkProvider.AddLink(addingLink);
            var addedLinkJson = new LinkJsonResponseModel
            {
                Id = addedLink.Id,
                Url = addedLink.Url,
                Description = addedLink.Description
            };

            return Json(addedLinkJson);
        }

        [HttpPut]
        public JsonResult Update(LinkJsonRequestModel linkJson)
        {
            var updatingLink = new Link
            {
                Id = linkJson.Id,
                Url = linkJson.Url.Trim(),
                Description = linkJson.Description.TrimAndUppercaseFirst(),
                TraineeProfileId = linkJson.TraineeProfileId
            };

            var updatedLink = _linkProvider.UpdateLink(updatingLink);
            var updatedLinkJson = new LinkJsonResponseModel
            {
                Id = updatedLink.Id,
                Url = updatedLink.Url,
                Description = updatedLink.Description
            };

            return Json(updatedLinkJson);
        }

        [HttpDelete]
        public void Remove(int id)
        {
            _linkProvider.RemoveLink(id);
        }
    }
}