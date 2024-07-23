using Core.Entities.Files.FileDataExtractTypes;

namespace Core.Services
{
    public class GradeService : IGradeService
    {
        public float GetStudentAverage(float grade1, float grade2, float grade3, float grade4)
        {
            var sumOfGrades = grade1 + grade2 + grade3 + grade4;
            float average = 0;
            try
            {
                average = sumOfGrades / 4;
            }
            catch 
            {
            }
            
            return average;
        }
    }
}
