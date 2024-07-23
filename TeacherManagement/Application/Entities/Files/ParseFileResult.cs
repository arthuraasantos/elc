namespace Core.Entities.Files
{
    public class ParseFileResult
    {
        public string ExtractedData {get; set; }
        public byte[]? Thumbnail { get; set; }
    }
}
