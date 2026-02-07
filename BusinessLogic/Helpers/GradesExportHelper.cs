using ClosedXML.Excel;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Helpers
{
    public class GradesExportHelper
    {
        private readonly EduPlusDbContext _context;

        public GradesExportHelper(EduPlusDbContext context)
        {
            _context = context;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<GradesExportData> GetGradesDataAsync(int classId, int subjectId, int semesterId, int schoolYearId, int? studentId = null)
        {
            var classEntity = await _context.Classes.FirstOrDefaultAsync(c => c.Id == classId);
            var className = classEntity != null ? $"{classEntity.Level}{classEntity.Letter}" : "Klasa";

            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == subjectId);
            var subjectName = subject?.Name ?? "Przedmiot";

            var semester = await _context.Semesters.FirstOrDefaultAsync(s => s.Id == semesterId);
            var semesterName = semester?.Name ?? "";

            var schoolYear = await _context.SchoolYears.FirstOrDefaultAsync(y => y.Id == schoolYearId);
            var schoolYearName = schoolYear?.Name ?? "";

            DateOnly startDate = semester?.StartDate ?? DateOnly.MinValue;
            DateOnly endDate = semester?.EndDate ?? DateOnly.MaxValue;
            var start = startDate.ToDateTime(TimeOnly.MinValue);
            var end = endDate.ToDateTime(TimeOnly.MaxValue);

            var query = _context.ClassStudents.AsNoTracking()
                .Where(cs => cs.ClassId == classId);

            if (studentId.HasValue)
                query = query.Where(cs => cs.StudentId == studentId.Value);

            var students = await query
                .OrderBy(cs => cs.OrderNumber)
                .Select(cs => new StudentGradeRow
                {
                    StudentId = cs.StudentId,
                    OrderNumber = cs.OrderNumber,
                    StudentName = $"{cs.Student.LastName} {cs.Student.FirstName}",
                    Grades = _context.Grades
                        .Where(g => g.StudentId == cs.StudentId && g.SubjectId == subjectId && g.IsActive && g.CreatedAt >= start && g.CreatedAt <= end)
                        .OrderBy(g => g.CreatedAt)
                        .Select(g => new GradeInfo
                        {
                            Value = g.GradeType.Numeric,
                            NumericValue = g.GradeType.Value,
                            Category = g.GradeCategory.Name,
                            Weight = g.GradeCategory.Weight
                        }).ToList(),
                    Average = EduPlusDbContext.CalculateWeightedAverage(cs.StudentId, subjectId, start, end)
                })
                .ToListAsync();

            return new GradesExportData
            {
                ClassName = className,
                SubjectName = subjectName,
                SemesterName = semesterName,
                SchoolYearName = schoolYearName,
                Students = students,
                IsSingleStudent = studentId.HasValue
            };
        }

        public byte[] GeneratePdf(GradesExportData data)
        {
            var header = $"Wykaz ocen - {data.ClassName} - {data.SubjectName} ({data.SemesterName}, {data.SchoolYearName})";

            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(1, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().PaddingBottom(8)
                        .AlignCenter()
                        .Text(header)
                        .SemiBold()
                        .FontSize(14);

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(25);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(8);
                            columns.ConstantColumn(50);
                        });

                        table.Header(h =>
                        {
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text("Lp").FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).Text("Uczeń").FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).Text("Oceny").FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text("Średnia").FontSize(9).SemiBold();
                        });

                        int lp = 0;
                        foreach (var row in data.Students)
                        {
                            lp++;
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(lp.ToString()).FontSize(9);
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).Text(row.StudentName).FontSize(9);
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).Text(string.Join(", ", row.Grades.Select(g => g.Value))).FontSize(9);
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(row.Average?.ToString("0.00") ?? "-").FontSize(9).SemiBold();
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Wygenerowano: ").FontSize(8);
                        x.Span(DateTime.Now.ToString("dd.MM.yyyy HH:mm")).FontSize(8);
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GenerateXlsx(GradesExportData data)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Oceny");

            var header = $"Wykaz ocen - {data.ClassName} - {data.SubjectName} ({data.SemesterName}, {data.SchoolYearName})";
            ws.Cell(1, 1).Value = header;
            ws.Range(1, 1, 1, 4).Merge().Style.Font.SetBold().Font.FontSize = 14;

            ws.Cell(3, 1).Value = "Lp";
            ws.Cell(3, 2).Value = "Uczeń";
            ws.Cell(3, 3).Value = "Oceny";
            ws.Cell(3, 4).Value = "Średnia";
            ws.Range(3, 1, 3, 4).Style.Font.SetBold().Fill.BackgroundColor = XLColor.LightGray;

            int row = 4;
            int lp = 0;
            foreach (var student in data.Students)
            {
                lp++;
                ws.Cell(row, 1).Value = lp;
                ws.Cell(row, 2).Value = student.StudentName;
                ws.Cell(row, 3).Value = string.Join(", ", student.Grades.Select(g => g.Value));
                ws.Cell(row, 4).Value = student.Average?.ToString("0.00") ?? "-";
                row++;
            }

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            ws.Column(3).Width = 50;
            ws.Column(4).Width = 10;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public string GenerateFileName(GradesExportData data, string extension) =>
            $"{DateTime.Now:yyyyMMddHHmmss}-wykaz-ocen-{data.ClassName.ToLower()}-{data.SubjectName.ToLower().Replace(" ", "-")}.{extension}";

        public async Task<StudentExportData> GetStudentGradesDataAsync(int studentId, int classId, int semesterId, int schoolYearId)
        {
            var student = await _context.Users.FirstOrDefaultAsync(u => u.Id == studentId);
            var studentName = student != null ? $"{student.LastName} {student.FirstName}" : "Uczeń";

            var classEntity = await _context.Classes.FirstOrDefaultAsync(c => c.Id == classId);
            var className = classEntity != null ? $"{classEntity.Level}{classEntity.Letter}" : "Klasa";

            var semester = await _context.Semesters.FirstOrDefaultAsync(s => s.Id == semesterId);
            var semesterName = semester?.Name ?? "";

            var schoolYear = await _context.SchoolYears.FirstOrDefaultAsync(y => y.Id == schoolYearId);
            var schoolYearName = schoolYear?.Name ?? "";

            DateOnly startDate = semester?.StartDate ?? DateOnly.MinValue;
            DateOnly endDate = semester?.EndDate ?? DateOnly.MaxValue;
            var start = startDate.ToDateTime(TimeOnly.MinValue);
            var end = endDate.ToDateTime(TimeOnly.MaxValue);

            var classSubjects = await _context.ClassSubjects.AsNoTracking()
                .Where(cs => cs.ClassId == classId)
                .Include(cs => cs.Subject)
                .OrderBy(cs => cs.Subject.Name)
                .ToListAsync();

            var subjects = new List<SubjectGradeRow>();
            foreach (var cs in classSubjects)
            {
                var grades = await _context.Grades.AsNoTracking()
                    .Where(g => g.StudentId == studentId && g.SubjectId == cs.SubjectId && g.IsActive && g.CreatedAt >= start && g.CreatedAt <= end)
                    .OrderBy(g => g.CreatedAt)
                    .Select(g => new GradeInfo
                    {
                        Value = g.GradeType.Numeric,
                        NumericValue = g.GradeType.Value,
                        Category = g.GradeCategory.Name,
                        Weight = g.GradeCategory.Weight
                    }).ToListAsync();

                decimal? average = null;
                if (grades.Any())
                {
                    var totalWeight = grades.Sum(g => g.Weight);
                    if (totalWeight > 0)
                        average = grades.Sum(g => g.NumericValue * g.Weight) / totalWeight;
                }

                subjects.Add(new SubjectGradeRow
                {
                    SubjectId = cs.SubjectId,
                    SubjectName = cs.Subject?.Name ?? "",
                    Grades = grades,
                    Average = average
                });
            }

            return new StudentExportData
            {
                StudentId = studentId,
                StudentName = studentName,
                ClassName = className,
                SemesterName = semesterName,
                SchoolYearName = schoolYearName,
                Subjects = subjects
            };
        }

        public byte[] GenerateStudentPdf(StudentExportData data)
        {
            var header = $"Wykaz ocen - {data.StudentName} ({data.ClassName}, {data.SemesterName}, {data.SchoolYearName})";

            var document = QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(1, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().PaddingBottom(8)
                        .AlignCenter()
                        .Text(header)
                        .SemiBold()
                        .FontSize(14);

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(25);
                            columns.RelativeColumn(3);
                            columns.RelativeColumn(8);
                            columns.ConstantColumn(50);
                        });

                        table.Header(h =>
                        {
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text("Lp").FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).Text("Przedmiot").FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).Text("Oceny").FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text("Średnia").FontSize(9).SemiBold();
                        });

                        int lp = 0;
                        foreach (var row in data.Subjects)
                        {
                            lp++;
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(lp.ToString()).FontSize(9);
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).Text(row.SubjectName).FontSize(9);
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).Text(string.Join(", ", row.Grades.Select(g => g.Value))).FontSize(9);
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(row.Average?.ToString("0.00") ?? "-").FontSize(9).SemiBold();
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Wygenerowano: ").FontSize(8);
                        x.Span(DateTime.Now.ToString("dd.MM.yyyy HH:mm")).FontSize(8);
                    });
                });
            });

            return document.GeneratePdf();
        }

        public byte[] GenerateStudentXlsx(StudentExportData data)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Oceny");

            var header = $"Wykaz ocen - {data.StudentName} ({data.ClassName}, {data.SemesterName}, {data.SchoolYearName})";
            ws.Cell(1, 1).Value = header;
            ws.Range(1, 1, 1, 4).Merge().Style.Font.SetBold().Font.FontSize = 14;

            ws.Cell(3, 1).Value = "Lp";
            ws.Cell(3, 2).Value = "Przedmiot";
            ws.Cell(3, 3).Value = "Oceny";
            ws.Cell(3, 4).Value = "Średnia";
            ws.Range(3, 1, 3, 4).Style.Font.SetBold().Fill.BackgroundColor = XLColor.LightGray;

            int row = 4;
            int lp = 0;
            foreach (var subject in data.Subjects)
            {
                lp++;
                ws.Cell(row, 1).Value = lp;
                ws.Cell(row, 2).Value = subject.SubjectName;
                ws.Cell(row, 3).Value = string.Join(", ", subject.Grades.Select(g => g.Value));
                ws.Cell(row, 4).Value = subject.Average?.ToString("0.00") ?? "-";
                row++;
            }

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            ws.Column(3).Width = 50;
            ws.Column(4).Width = 10;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public string GenerateStudentFileName(StudentExportData data, string extension) =>
            $"{DateTime.Now:yyyyMMddHHmmss}-wykaz-ocen-{data.StudentName.ToLower().Replace(" ", "-")}.{extension}";
    }

    public class GradesExportData
    {
        public string ClassName { get; set; } = "";
        public string SubjectName { get; set; } = "";
        public string SemesterName { get; set; } = "";
        public string SchoolYearName { get; set; } = "";
        public List<StudentGradeRow> Students { get; set; } = new();
        public bool IsSingleStudent { get; set; }
    }

    public class StudentExportData
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = "";
        public string ClassName { get; set; } = "";
        public string SemesterName { get; set; } = "";
        public string SchoolYearName { get; set; } = "";
        public List<SubjectGradeRow> Subjects { get; set; } = new();
    }

    public class StudentGradeRow
    {
        public int StudentId { get; set; }
        public int OrderNumber { get; set; }
        public string StudentName { get; set; } = "";
        public List<GradeInfo> Grades { get; set; } = new();
        public decimal? Average { get; set; }
    }

    public class SubjectGradeRow
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = "";
        public List<GradeInfo> Grades { get; set; } = new();
        public decimal? Average { get; set; }
    }

    public class GradeInfo
    {
        public string Value { get; set; } = "";
        public decimal NumericValue { get; set; }
        public string Category { get; set; } = "";
        public int Weight { get; set; }
    }
}
