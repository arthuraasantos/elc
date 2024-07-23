using Core.Entities.Files.FileDataExtractTypes;
using Core.Services;

namespace Core.Tests.Grades
{
    public class ClassTests
    {
        [Fact]
        public void GetApprovalPercentageInClassFullApproved()
        {
            const float EXPECTED_APPROVAL_PERCENTAGE = 100.0f;
            var students = new List<ClassEvaluationStudentFileData>() 
            {
                new ClassEvaluationStudentFileData(){ Id = "1", Name = "Student 1", Grade1 = 9.5f, Grade2 = 9.5f, Grade3 = 9.5f, Grade4 = 9.5f, GradeAverage = 9.5f },
                new ClassEvaluationStudentFileData(){ Id = "2", Name = "Student 2", Grade1 = 8.5f, Grade2 = 8.5f, Grade3 = 8.5f, Grade4 = 8.5f, GradeAverage = 8.5f },
                new ClassEvaluationStudentFileData(){ Id = "3", Name = "Student 3", Grade1 = 8.5f, Grade2 = 8.5f, Grade3 = 8.5f, Grade4 = 8.5f, GradeAverage = 8.5f },
                new ClassEvaluationStudentFileData(){ Id = "4", Name = "Student 4", Grade1 = 8.5f, Grade2 = 8.5f, Grade3 = 8.5f, Grade4 = 8.5f, GradeAverage = 8.5f }
            };

            var classService = new ClassService();

            var approvalPercentage = classService.GetApprovalPercentage(students);

            Assert.True(approvalPercentage > 0);
            Assert.Equal(EXPECTED_APPROVAL_PERCENTAGE, approvalPercentage);
        }

        [Fact]
        public void GetApprovalPercentageInClassPartialApproved()
        {
            const float EXPECTED_APPROVAL_PERCENTAGE = 75.0f;
            var students = new List<ClassEvaluationStudentFileData>()
            {
                new ClassEvaluationStudentFileData(){ Id = "1", Name = "Student 1", Grade1 = 9.5f, Grade2 = 9.5f, Grade3 = 9.5f, Grade4 = 9.5f, GradeAverage = 9.5f },
                new ClassEvaluationStudentFileData(){ Id = "2", Name = "Student 2", Grade1 = 8.5f, Grade2 = 8.5f, Grade3 = 8.5f, Grade4 = 8.5f, GradeAverage = 8.5f },
                new ClassEvaluationStudentFileData(){ Id = "3", Name = "Student 3", Grade1 = 7.0f, Grade2 = 7.0f, Grade3 = 7.0f, Grade4 = 7.0f, GradeAverage = 7.0f },
                new ClassEvaluationStudentFileData(){ Id = "4", Name = "Student 4", Grade1 = 6.5f, Grade2 = 6.5f, Grade3 = 6.5f, Grade4 = 6.5f, GradeAverage = 6.5f }
            };

            var classService = new ClassService();

            var approvalPercentage = classService.GetApprovalPercentage(students);

            Assert.True(approvalPercentage > 0);
            Assert.Equal(EXPECTED_APPROVAL_PERCENTAGE, approvalPercentage);
        }

        [Fact]
        public void GetApprovalPercentageInClassFullReproved()
        {
            const float EXPECTED_APPROVAL_PERCENTAGE = 0.0f;
            var students = new List<ClassEvaluationStudentFileData>()
            {
                new ClassEvaluationStudentFileData(){ Id = "1", Name = "Student 1", Grade1 = 6.5f, Grade2 = 6.5f, Grade3 = 6.5f, Grade4 = 6.5f, GradeAverage = 6.5f },
                new ClassEvaluationStudentFileData(){ Id = "2", Name = "Student 2", Grade1 = 6.5f, Grade2 = 6.5f, Grade3 = 6.5f, Grade4 = 6.5f, GradeAverage = 6.5f },
                new ClassEvaluationStudentFileData(){ Id = "3", Name = "Student 3", Grade1 = 6.5f, Grade2 = 6.5f, Grade3 = 6.5f, Grade4 = 6.5f, GradeAverage = 6.5f },
                new ClassEvaluationStudentFileData(){ Id = "4", Name = "Student 4", Grade1 = 6.5f, Grade2 = 6.5f, Grade3 = 6.5f, Grade4 = 6.5f, GradeAverage = 6.5f }
            };

            var classService = new ClassService();

            var approvalPercentage = classService.GetApprovalPercentage(students);

            Assert.Equal(EXPECTED_APPROVAL_PERCENTAGE, approvalPercentage);
        }

        [Fact]
        public void GetClassBestStudent()
        {
            var expectedBestStudent = new ClassEvaluationStudentFileData()
            {
                Id = "1",
                Name = "Student 1",
                Grade1 = 9.5f,
                Grade2 = 9.5f,
                Grade3 = 9.5f,
                Grade4 = 9.5f,
                GradeAverage = 9.5f
            };

            var students = new List<ClassEvaluationStudentFileData>()
            {
                expectedBestStudent,
                new ClassEvaluationStudentFileData(){ Id = "2", Name = "Student 2", Grade1 = 8.5f, Grade2 = 8.5f, Grade3 = 8.5f, Grade4 = 8.5f, GradeAverage = 8.5f },
                new ClassEvaluationStudentFileData(){ Id = "3", Name = "Student 3", Grade1 = 8.5f, Grade2 = 8.5f, Grade3 = 8.5f, Grade4 = 8.5f, GradeAverage = 8.5f },
                new ClassEvaluationStudentFileData(){ Id = "4", Name = "Student 4", Grade1 = 8.5f, Grade2 = 8.5f, Grade3 = 8.5f, Grade4 = 8.5f, GradeAverage = 8.5f }
            };

            var classService = new ClassService();

            var bestStudent = classService.GetClassBestStudent(students);

            Assert.NotNull(bestStudent);
            Assert.Equal(expectedBestStudent.Id, bestStudent);
        }

        [Fact]
        public void GetClassAverage()
        {
            const float EXPECTED_CLASS_AVERAGE = 8.5f;
            var students = new List<ClassEvaluationStudentFileData>()
            {
                new ClassEvaluationStudentFileData(){ Id = "2", Name = "Student 2", Grade1 = 8.5f, Grade2 = 8.5f, Grade3 = 8.5f, Grade4 = 8.5f, GradeAverage = 8.5f },
                new ClassEvaluationStudentFileData(){ Id = "3", Name = "Student 3", Grade1 = 8.5f, Grade2 = 8.5f, Grade3 = 8.5f, Grade4 = 8.5f, GradeAverage = 8.5f },
                new ClassEvaluationStudentFileData(){ Id = "4", Name = "Student 4", Grade1 = 8.5f, Grade2 = 8.5f, Grade3 = 8.5f, Grade4 = 8.5f, GradeAverage = 8.5f }
            };

            var classService = new ClassService();

            var classAverage = classService.GetClassGradeAverage(students);

            Assert.Equal(EXPECTED_CLASS_AVERAGE, classAverage);
        }
    }
}