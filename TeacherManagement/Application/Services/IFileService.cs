using Core.Entities.Files;

namespace Core.Services
{
    public interface IFileService
    {
        Task<ParseFileResult> ReadFile(ParseFileTypes parseType, string filePath, bool generateThumbNail = true);
        string GetFileExtension(string fileName);
        string GetFileContentType(string fileName);
        string GetPdfFromXLS(MyFile file, string userUploadFolderPath, string excelToDownload);
    }
}
