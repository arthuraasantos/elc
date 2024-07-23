using Core.Entities.Files;
using Web.Models.Files;

namespace Web.Adapters
{
    public static class FileAdapter
    {
        public static FileManagementModel ToManagementModel(List<MyFile> files)
        {
            return new FileManagementModel()
            {
                Files = ToFileManagementModel(files)
            };
        }

        public static List<FileItemManagementModel>? ToFileManagementModel(List<MyFile> files)
        {
            if(files == null || files.Count == 0) 
                return new List<FileItemManagementModel>();

            return files?.Select(f => new FileItemManagementModel()
            {
                Id = f.Id,
                Description = f.Description,
                OriginalName = f.OriginalName,
                OwnerId = f.OwnerId,
                Size = f.Size,
                Downloads = f.Downloads,
                Extension = f.Extension,
                CreatedAt = f.CreatedAt,
                CreatedUser = f.CreatedUser,
                Data = new FileItemDataManagementModel()
                {
                    ExtractedData = f.Data?.ExtractedData,
                    Thumbnail = f.Data?.Thumbnail
                }
            })?.ToList();
        }
    }
}
