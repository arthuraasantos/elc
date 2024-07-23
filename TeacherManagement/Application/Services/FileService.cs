using ClosedXML.Excel;
using Core.Entities.Files;
using Core.Entities.Files.FileDataExtractTypes;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using SkiaSharp;

namespace Core.Services
{
    public class FileService(IClassService classService, IGradeService gradeService) : IFileService
    {
        private readonly IClassService _classService = classService;
        private readonly IGradeService _gradeService = gradeService;

        public async Task<ParseFileResult> ReadFile(ParseFileTypes parseType, string filePath, bool generateThumbNail = true)
        {
            var parsedResult = new ParseFileResult();

            using (var workbook = new XLWorkbook(filePath))
            {
                var firstWorksheet = workbook.Worksheets.First();

                

                switch (parseType)
                {
                    case ParseFileTypes.GradeStudents:
                    default:
                        parsedResult = ParseGradeStudentFile(firstWorksheet, generateThumbNail);
                        break;
                }
            }

            return await Task.FromResult(parsedResult);
        }

        private ParseFileResult ParseGradeStudentFile(IXLWorksheet worksheet, bool generateThumbNail = true)
        {
            var parsedResult = new ParseFileResult();

            if (generateThumbNail)
                parsedResult.Thumbnail = GenerateThumbnail(worksheet);

            const string CELLADDRESS_SUBJECT = "A2";
            const string COLUMN_STUDENT_ID = "B";
            const string COLUMN_STUDENT_NAME = "C";
            const string COLUMN_STUDENT_GRADE1 = "D";
            const string COLUMN_STUDENT_GRADE2 = "E";
            const string COLUMN_STUDENT_GRADE3 = "F";
            const string COLUMN_STUDENT_GRADE4 = "G";
            const int TITLE_LINES_TO_SKIP = 1;

            var gradeStudentFileData = new ClassEvaluationFileData()
            {
                Students = new()
            };
            
            var teacherRow = worksheet.RowsUsed().First();
            var teacherCell = teacherRow.FirstCell();
            gradeStudentFileData.Teacher = teacherCell.Value.ToString();

            foreach (var row in worksheet.RowsUsed().Skip(TITLE_LINES_TO_SKIP))
            {
                var student = new ClassEvaluationStudentFileData()
                {
                    Grade1 = 0,
                    Grade2 = 0,
                    Grade3 = 0,
                    Grade4 = 0
                };

                foreach (var cell in row.CellsUsed())
                {
                    var columnLetter = cell.Address.ColumnLetter;
                    var cellAddress = cell.Address.ToString();

                    if (CELLADDRESS_SUBJECT == cellAddress)
                    {
                        const string SUBJECT_PREFIX = "Matéria: ";
                        gradeStudentFileData.Subject = cell.Value.ToString().Replace(SUBJECT_PREFIX, string.Empty);
                        continue;
                    }

                    switch (columnLetter)
                    {
                        case COLUMN_STUDENT_ID: student.Id = cell.Value.ToString(); continue;
                        case COLUMN_STUDENT_NAME: student.Name = cell.Value.ToString(); continue;
                        case COLUMN_STUDENT_GRADE1: student.Grade1 = float.TryParse(cell.Value.ToString(), out float grade1Result) ? grade1Result: 0; continue;
                        case COLUMN_STUDENT_GRADE2: student.Grade2 = float.TryParse(cell.Value.ToString(), out float grade2Result) ? grade2Result: 0; continue;
                        case COLUMN_STUDENT_GRADE3: student.Grade3 = float.TryParse(cell.Value.ToString(), out float grade3Result) ? grade3Result: 0; continue;
                        case COLUMN_STUDENT_GRADE4: student.Grade4 = float.TryParse(cell.Value.ToString(), out float grade4Result) ? grade4Result : 0; continue;
                        default: break;
                    }
                }

                student.GradeAverage = _gradeService.GetStudentAverage(student.Grade1.Value, student.Grade2.Value, student.Grade3.Value, student.Grade4.Value);

                gradeStudentFileData.Students.Add(student);

            }

            gradeStudentFileData.ApprovalPercentage = _classService.GetApprovalPercentage(gradeStudentFileData.Students);
            gradeStudentFileData.Average = _classService.GetClassGradeAverage(gradeStudentFileData.Students);
            gradeStudentFileData.BestStudentId = _classService.GetClassBestStudent(gradeStudentFileData.Students);

            parsedResult.ExtractedData = JsonConvert.SerializeObject(gradeStudentFileData);
            
            return parsedResult;
        }

        private byte[] GenerateThumbnail(IXLWorksheet worksheet, int width = 200, int height = 100)
        {
            using var bitmap = new SKBitmap(width, height);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            const int NUMBER_TO_SHOW_IN_THUMB = 3;
            int rowCount = Math.Min(worksheet.RowsUsed().Count(), NUMBER_TO_SHOW_IN_THUMB); 
            int colCount = Math.Min(worksheet.ColumnsUsed().Count(), NUMBER_TO_SHOW_IN_THUMB); 
            int cellWidth = width / colCount;
            int cellHeight = height / rowCount;

            using var paint = new SKPaint
            {
                Color = SKColors.Black,
                IsAntialias = true,
                TextSize = 9,
                Typeface = SKTypeface.Default,
            };

            int y = 0;
            foreach (var row in worksheet.RowsUsed().Take(rowCount))
            {
                int x = 0;
                foreach (var cell in row.Cells().Take(colCount))
                {
                    var cellValue = cell.GetString();

                    canvas.DrawText(cellValue, x + 5, y + cellHeight / 2 + paint.TextSize / 2, paint);

                    x += cellWidth;
                }
                y += cellHeight;
            }

            using var imageStream = new MemoryStream();
            using var skImage = SKImage.FromBitmap(bitmap);
            using var data = skImage.Encode(SKEncodedImageFormat.Png, 100);
            data.SaveTo(imageStream);

            return imageStream.ToArray();
        }
    }
}
