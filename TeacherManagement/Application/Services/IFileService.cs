using Core.Entities.Files;

namespace Core.Services
{
    public interface IFileService
    {
        Task<ParseFileResult> ReadFile(ParseFileTypes parseType, string filePath, bool generateThumbNail = true);
    }
}
