using Core.Entities.Files.FileDataExtractTypes;

namespace Core.Services
{
    public interface IGradeService
    {
        float GetStudentAverage(float grade1, float grade2, float grade3, float grade4);
    }
}
