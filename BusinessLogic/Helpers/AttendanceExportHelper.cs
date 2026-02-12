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
    public class AttendanceExportHelper
    {
        private readonly EduPlusDbContext _context;

        public AttendanceExportHelper(EduPlusDbContext context)
        {
            _context = context;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<AttendanceExportData> GetAttendanceDataAsync(int classId, int semesterId, int schoolYearId, int? studentId = null)
        {
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

            var attendanceTypes = await _context.Set<Data.Data.Entities.AttendanceType>()
                .Where(at => at.IsActive)
                .OrderBy(at => at.Id)
                .Select(at => new AttendanceTypeInfo { Id = at.Id, Name = at.Name, ShortCode = at.ShortCode })
                .ToListAsync();

            var query = _context.ClassStudents.AsNoTracking()
                .Where(cs => cs.ClassId == classId && cs.IsActive);

            if (studentId.HasValue)
                query = query.Where(cs => cs.StudentId == studentId.Value);

            var classStudents = await query
                .OrderBy(cs => cs.OrderNumber)
                .Select(cs => new { cs.StudentId, cs.OrderNumber, StudentName = $"{cs.Student.LastName} {cs.Student.FirstName}" })
                .ToListAsync();

            var studentIds = classStudents.Select(s => s.StudentId).ToList();

            var attendances = await _context.Set<Data.Data.Entities.Attendance>()
                .AsNoTracking()
                .Where(a => a.IsActive && studentIds.Contains(a.StudentId)
                    && a.Lesson.Date >= start && a.Lesson.Date <= end
                    && a.Lesson.ClassId == classId)
                .Select(a => new { a.StudentId, a.AttendanceTypeId })
                .ToListAsync();

            var grouped = attendances.GroupBy(a => a.StudentId)
                .ToDictionary(g => g.Key, g => g.GroupBy(a => a.AttendanceTypeId).ToDictionary(gg => gg.Key, gg => gg.Count()));

            var rows = classStudents.Select(cs =>
            {
                var counts = new Dictionary<int, int>();
                int total = 0;
                foreach (var at in attendanceTypes)
                {
                    var count = grouped.ContainsKey(cs.StudentId) && grouped[cs.StudentId].ContainsKey(at.Id) ? grouped[cs.StudentId][at.Id] : 0;
                    counts[at.Id] = count;
                    total += count;
                }
                var presentId = attendanceTypes.FirstOrDefault(t => t.ShortCode == "OB")?.Id ?? 1;
                var presentCount = counts.ContainsKey(presentId) ? counts[presentId] : 0;
                var percentage = total > 0 ? (decimal)presentCount / total * 100 : 0;

                return new StudentAttendanceRow
                {
                    StudentId = cs.StudentId,
                    OrderNumber = cs.OrderNumber,
                    StudentName = cs.StudentName,
                    TypeCounts = counts,
                    Total = total,
                    PresencePercentage = percentage
                };
            }).ToList();

            return new AttendanceExportData
            {
                ClassName = className,
                SemesterName = semesterName,
                SchoolYearName = schoolYearName,
                AttendanceTypes = attendanceTypes,
                Students = rows
            };
        }

        public async Task<StudentAttendanceExportData> GetStudentAttendanceDataAsync(int studentId, int classId, int semesterId, int schoolYearId)
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

            var attendanceTypes = await _context.Set<Data.Data.Entities.AttendanceType>()
                .Where(at => at.IsActive)
                .OrderBy(at => at.Id)
                .Select(at => new AttendanceTypeInfo { Id = at.Id, Name = at.Name, ShortCode = at.ShortCode })
                .ToListAsync();

            var subjects = await _context.ClassSubjects.AsNoTracking()
                .Where(cs => cs.ClassId == classId && cs.IsActive)
                .OrderBy(cs => cs.Subject.Name)
                .Select(cs => new { cs.SubjectId, SubjectName = cs.Subject.Name ?? "" })
                .ToListAsync();

            var subjectIds = subjects.Select(s => s.SubjectId).ToList();

            var attendances = await _context.Set<Data.Data.Entities.Attendance>()
                .AsNoTracking()
                .Where(a => a.IsActive && a.StudentId == studentId
                    && a.Lesson.Date >= start && a.Lesson.Date <= end
                    && a.Lesson.ClassId == classId
                    && subjectIds.Contains(a.Lesson.SubjectId))
                .Select(a => new { a.Lesson.SubjectId, a.AttendanceTypeId })
                .ToListAsync();

            var grouped = attendances.GroupBy(a => a.SubjectId)
                .ToDictionary(g => g.Key, g => g.GroupBy(a => a.AttendanceTypeId).ToDictionary(gg => gg.Key, gg => gg.Count()));

            var rows = subjects.Select((s, idx) =>
            {
                var counts = new Dictionary<int, int>();
                int total = 0;
                foreach (var at in attendanceTypes)
                {
                    var count = grouped.ContainsKey(s.SubjectId) && grouped[s.SubjectId].ContainsKey(at.Id) ? grouped[s.SubjectId][at.Id] : 0;
                    counts[at.Id] = count;
                    total += count;
                }
                var presentId = attendanceTypes.FirstOrDefault(t => t.ShortCode == "OB")?.Id ?? 1;
                var presentCount = counts.ContainsKey(presentId) ? counts[presentId] : 0;
                var percentage = total > 0 ? (decimal)presentCount / total * 100 : 0;

                return new SubjectAttendanceRow
                {
                    SubjectId = s.SubjectId,
                    SubjectName = s.SubjectName,
                    TypeCounts = counts,
                    Total = total,
                    PresencePercentage = percentage
                };
            }).ToList();

            return new StudentAttendanceExportData
            {
                StudentId = studentId,
                StudentName = studentName,
                ClassName = className,
                SemesterName = semesterName,
                SchoolYearName = schoolYearName,
                AttendanceTypes = attendanceTypes,
                Subjects = rows
            };
        }

        public byte[] GeneratePdf(AttendanceExportData data)
        {
            var header = $"Wykaz frekwencji - {data.ClassName} ({data.SemesterName}, {data.SchoolYearName})";
            var types = data.AttendanceTypes;

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
                            columns.RelativeColumn(4);
                            foreach (var _ in types) columns.ConstantColumn(40);
                            columns.ConstantColumn(40);
                            columns.ConstantColumn(50);
                        });

                        table.Header(h =>
                        {
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text("Lp").FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).Text("Uczeń").FontSize(9).SemiBold();
                            foreach (var at in types)
                                h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(at.ShortCode).FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text("Suma").FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text("% obecn.").FontSize(9).SemiBold();
                        });

                        int lp = 0;
                        foreach (var row in data.Students)
                        {
                            lp++;
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(lp.ToString()).FontSize(9);
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).Text(row.StudentName).FontSize(9);
                            foreach (var at in types)
                                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(row.TypeCounts.GetValueOrDefault(at.Id, 0).ToString()).FontSize(9);
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(row.Total.ToString()).FontSize(9).SemiBold();
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(row.PresencePercentage.ToString("0.0") + "%").FontSize(9).SemiBold();
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

        public byte[] GenerateXlsx(AttendanceExportData data)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Frekwencja");
            var types = data.AttendanceTypes;

            var header = $"Wykaz frekwencji - {data.ClassName} ({data.SemesterName}, {data.SchoolYearName})";
            var totalCols = 2 + types.Count + 2;
            ws.Cell(1, 1).Value = header;
            ws.Range(1, 1, 1, totalCols).Merge().Style.Font.SetBold().Font.FontSize = 14;

            ws.Cell(3, 1).Value = "Lp";
            ws.Cell(3, 2).Value = "Uczeń";
            for (int i = 0; i < types.Count; i++)
                ws.Cell(3, 3 + i).Value = types[i].ShortCode;
            ws.Cell(3, 3 + types.Count).Value = "Suma";
            ws.Cell(3, 4 + types.Count).Value = "% obecn.";
            ws.Range(3, 1, 3, totalCols).Style.Font.SetBold().Fill.BackgroundColor = XLColor.LightGray;

            int row = 4;
            int lp = 0;
            foreach (var student in data.Students)
            {
                lp++;
                ws.Cell(row, 1).Value = lp;
                ws.Cell(row, 2).Value = student.StudentName;
                for (int i = 0; i < types.Count; i++)
                    ws.Cell(row, 3 + i).Value = student.TypeCounts.GetValueOrDefault(types[i].Id, 0);
                ws.Cell(row, 3 + types.Count).Value = student.Total;
                ws.Cell(row, 4 + types.Count).Value = student.PresencePercentage.ToString("0.0") + "%";
                row++;
            }

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            for (int i = 0; i < types.Count; i++) ws.Column(3 + i).Width = 8;
            ws.Column(3 + types.Count).Width = 8;
            ws.Column(4 + types.Count).Width = 10;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public byte[] GenerateStudentPdf(StudentAttendanceExportData data)
        {
            var header = $"Wykaz frekwencji - {data.StudentName} ({data.ClassName}, {data.SemesterName}, {data.SchoolYearName})";
            var types = data.AttendanceTypes;

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
                            columns.RelativeColumn(4);
                            foreach (var _ in types) columns.ConstantColumn(40);
                            columns.ConstantColumn(40);
                            columns.ConstantColumn(50);
                        });

                        table.Header(h =>
                        {
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text("Lp").FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).Text("Przedmiot").FontSize(9).SemiBold();
                            foreach (var at in types)
                                h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(at.ShortCode).FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text("Suma").FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text("% obecn.").FontSize(9).SemiBold();
                        });

                        int lp = 0;
                        foreach (var row in data.Subjects)
                        {
                            lp++;
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(lp.ToString()).FontSize(9);
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).Text(row.SubjectName).FontSize(9);
                            foreach (var at in types)
                                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(row.TypeCounts.GetValueOrDefault(at.Id, 0).ToString()).FontSize(9);
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(row.Total.ToString()).FontSize(9).SemiBold();
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(row.PresencePercentage.ToString("0.0") + "%").FontSize(9).SemiBold();
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

        public byte[] GenerateStudentXlsx(StudentAttendanceExportData data)
        {
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Frekwencja");
            var types = data.AttendanceTypes;

            var header = $"Wykaz frekwencji - {data.StudentName} ({data.ClassName}, {data.SemesterName}, {data.SchoolYearName})";
            var totalCols = 2 + types.Count + 2;
            ws.Cell(1, 1).Value = header;
            ws.Range(1, 1, 1, totalCols).Merge().Style.Font.SetBold().Font.FontSize = 14;

            ws.Cell(3, 1).Value = "Lp";
            ws.Cell(3, 2).Value = "Przedmiot";
            for (int i = 0; i < types.Count; i++)
                ws.Cell(3, 3 + i).Value = types[i].ShortCode;
            ws.Cell(3, 3 + types.Count).Value = "Suma";
            ws.Cell(3, 4 + types.Count).Value = "% obecn.";
            ws.Range(3, 1, 3, totalCols).Style.Font.SetBold().Fill.BackgroundColor = XLColor.LightGray;

            int row = 4;
            int lp = 0;
            foreach (var subject in data.Subjects)
            {
                lp++;
                ws.Cell(row, 1).Value = lp;
                ws.Cell(row, 2).Value = subject.SubjectName;
                for (int i = 0; i < types.Count; i++)
                    ws.Cell(row, 3 + i).Value = subject.TypeCounts.GetValueOrDefault(types[i].Id, 0);
                ws.Cell(row, 3 + types.Count).Value = subject.Total;
                ws.Cell(row, 4 + types.Count).Value = subject.PresencePercentage.ToString("0.0") + "%";
                row++;
            }

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 30;
            for (int i = 0; i < types.Count; i++) ws.Column(3 + i).Width = 8;
            ws.Column(3 + types.Count).Width = 8;
            ws.Column(4 + types.Count).Width = 10;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public string GenerateFileName(AttendanceExportData data, string extension) =>
            $"{DateTime.Now:yyyyMMddHHmmss}-wykaz-frekwencji-{data.ClassName.ToLower()}.{extension}";

        public string GenerateStudentFileName(StudentAttendanceExportData data, string extension) =>
            $"{DateTime.Now:yyyyMMddHHmmss}-wykaz-frekwencji-{data.StudentName.ToLower().Replace(" ", "-")}.{extension}";
    }

    public class AttendanceExportData
    {
        public string ClassName { get; set; } = "";
        public string SemesterName { get; set; } = "";
        public string SchoolYearName { get; set; } = "";
        public List<AttendanceTypeInfo> AttendanceTypes { get; set; } = new();
        public List<StudentAttendanceRow> Students { get; set; } = new();
    }

    public class StudentAttendanceExportData
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = "";
        public string ClassName { get; set; } = "";
        public string SemesterName { get; set; } = "";
        public string SchoolYearName { get; set; } = "";
        public List<AttendanceTypeInfo> AttendanceTypes { get; set; } = new();
        public List<SubjectAttendanceRow> Subjects { get; set; } = new();
    }

    public class StudentAttendanceRow
    {
        public int StudentId { get; set; }
        public int OrderNumber { get; set; }
        public string StudentName { get; set; } = "";
        public Dictionary<int, int> TypeCounts { get; set; } = new();
        public int Total { get; set; }
        public decimal PresencePercentage { get; set; }
    }

    public class SubjectAttendanceRow
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = "";
        public Dictionary<int, int> TypeCounts { get; set; } = new();
        public int Total { get; set; }
        public decimal PresencePercentage { get; set; }
    }

    public class AttendanceTypeInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string ShortCode { get; set; } = "";
    }
}
