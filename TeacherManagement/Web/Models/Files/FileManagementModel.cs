using Core.Entities.Files;

namespace Web.Models.Files
{
    public class FileManagementModel
    {
        public List<FileItemManagementModel>? Files { get; set; }

        public IFormFile NewFiles { get; set; }
    }
}
