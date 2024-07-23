namespace Core.Entities.Files.FileDataExtractTypes
{
    public class ClassEvaluationFileData
    {
        public string? Teacher { get; set; }
        public string? Subject { get; set; }
        public string? BestStudentId { get; set; }
        public List<ClassEvaluationStudentFileData> Students { get; set; }

        public float? ApprovalPercentage { get; set; }
        public float? Average { get; set; }
    }

    public class ClassEvaluationStudentFileData
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public float? Grade1 { get; set; }
        public float? Grade2 { get; set; }
        public float? Grade3 { get; set; }
        public float? Grade4 { get; set; }

        public float GradeAverage { get; set; }
    }
}
