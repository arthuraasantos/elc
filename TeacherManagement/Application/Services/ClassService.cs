using Core.Entities.Files.FileDataExtractTypes;

namespace Core.Services
{
    public class ClassService : IClassService
    {
        public float GetApprovalPercentage(List<ClassEvaluationStudentFileData> students)
        {
            if (students == null || students.Count == 0) return 0;

            const float APPROVAL_AVERAGE = 7.0f;

            var approvedStudents = students.Count(s => s.GradeAverage >= APPROVAL_AVERAGE);

            if(approvedStudents > 0)
            {
                return (approvedStudents * 100) / students.Count;
            }

            return 0;

        }

        public string GetClassBestStudent(List<ClassEvaluationStudentFileData> studentsData)
        {
            if (studentsData == null || studentsData.Count == 0) return null;

            var bestStudent = studentsData.MaxBy(s => s.GradeAverage);

            if (bestStudent == null) return null;

            return bestStudent.Id;
        }

        public float GetClassGradeAverage(List<ClassEvaluationStudentFileData> students)
        {
            if (students == null || students.Count == 0) return 0;

            var studentsTotalAverage = students.Sum(s => s.GradeAverage);
            
            return studentsTotalAverage / students.Count;
        }
    }
}
