using Core.Entities.Common;
using Newtonsoft.Json;

namespace Core.Entities.Files
{
    public class MyFileData : AuditEntity
    {
        public Guid FileId { get; set; }
        public MyFile File { get; set; }
        public string? ExtractedData { get; set; }
        
        public byte[]? Thumbnail { get; set; }

        public T GetFormattedData<T>()
            where T : class
        {
            if (!string.IsNullOrEmpty(ExtractedData))
                return JsonConvert.DeserializeObject<T>(ExtractedData);

            return default(T);
        }

    }
}
