using System.IO;
using System.Web;
using Newtonsoft.Json;
using Profile.UI.Attributes;

namespace Profile.UI.Models.Json
{
    [JsonObject]
    [JsonNetModel]
    public class FileJsonRequestModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("profileId")]
        public int ProfileId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("file")]
        public HttpPostedFileBase File { get; set; }

        public byte[] GetFileData()
        {
            byte[] fileData = null;

            if (File != null)
            {
                using (var binaryReader = new BinaryReader(File.InputStream))
                {
                    fileData = binaryReader.ReadBytes(File.ContentLength);
                } 
            }

            return fileData;
        }
    }
}