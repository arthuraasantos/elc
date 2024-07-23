using Core.Services;

namespace Core.Tests.Grades
{
    public class GradeTests
    {
        [Theory]
        [InlineData(8, 7.5, 9.5, 8.5, 8.375)]
        [InlineData(9, 8.5, 7.5, 9, 8.5)]
        [InlineData(6.5, 6, 5.5, 7, 6.25)]
        [InlineData(7.5, 8, 9, 7.5, 8)]
        [InlineData(8.5, 7, 8, 9, 8.125)]
        public void GetStudentAverage(float grade1, float grade2, float grade3, float grade4, float expectedAverage)
        {
            var gradeService = new GradeService();

            var averageResult = gradeService.GetStudentAverage(grade1, grade2, grade3, grade4);

            Assert.Equal(expectedAverage, averageResult);
        }
    }
}