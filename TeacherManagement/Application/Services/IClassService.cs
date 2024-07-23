using Core.Entities.Files.FileDataExtractTypes;

namespace Core.Services
{
    public interface IClassService
    {
        float GetApprovalPercentage(List<ClassEvaluationStudentFileData> students);
        string GetClassBestStudent(List<ClassEvaluationStudentFileData> studentsData);
        float GetClassGradeAverage(List<ClassEvaluationStudentFileData> students);
    }
}
