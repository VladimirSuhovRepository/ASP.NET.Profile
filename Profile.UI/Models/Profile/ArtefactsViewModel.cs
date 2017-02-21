using System.Collections.Generic;

namespace Profile.UI.Models.Profile
{
    public class ArtefactsViewModel
    {
        public ArtefactsViewModel()
        {
            Files = new List<FileViewModel>();
        }

        public List<FileViewModel> Files { get; set; }
        public List<LinkViewModel> Links { get; set; }
    }
}