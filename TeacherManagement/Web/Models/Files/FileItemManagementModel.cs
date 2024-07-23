namespace Web.Models.Files
{
    public class FileItemManagementModel
    {
        public Guid Id { get; set; }
        public string OriginalName { get; set; }
        public string? Description { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }
        public int Downloads { get; set; }
        public Guid OwnerId { get; set; }
        public string CreatedUser { get; set; }
        public DateTime CreatedAt { get; set; }

        public FileItemDataManagementModel? Data { get; set; }
    }
}
