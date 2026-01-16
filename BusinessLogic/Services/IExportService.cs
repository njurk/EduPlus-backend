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
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IExportService
    {
        Task<byte[]> ExportScheduleToPdfAsync(int classId, int? yearId, int? semesterId);
        Task<byte[]> ExportScheduleToXlsxAsync(int classId, int? yearId, int? semesterId);
        Task<byte[]> ExportScheduleToCsvAsync(int classId, int? yearId, int? semesterId);
    }

    public class ExportService : IExportService
    {
        private readonly EduPlusDbContext _context;
        private readonly string[] _dayNames = { "Niedziela", "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek", "Sobota" };

        public ExportService(EduPlusDbContext context)
        {
            _context = context;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        private async Task<(string ClassName, List<ScheduleRow> Rows)> GetScheduleDataAsync(int classId, int? yearId, int? semesterId)
        {
            var classEntity = await _context.Classes
                .FirstOrDefaultAsync(c => c.Id == classId);

            var className = classEntity != null ? $"{classEntity.Level}{classEntity.Letter}" : "Klasa";

            var query = _context.WeeklySchedules
                .Where(ws => ws.ClassId == classId && ws.IsActive)
                .Include(ws => ws.Subject)
                .Include(ws => ws.Teacher)
                .Include(ws => ws.Classroom)
                .Include(ws => ws.LessonHour)
                .AsQueryable();

            if (yearId.HasValue)
                query = query.Where(ws => ws.SchoolYearId == yearId.Value);

            if (semesterId.HasValue)
                query = query.Where(ws => ws.SemesterId == semesterId.Value);

            var schedules = await query
                .OrderBy(ws => ws.LessonHour.OrderNumber)
                .ThenBy(ws => ws.DayOfWeek)
                .ToListAsync();

            var rows = schedules.Select(s => new ScheduleRow
            {
                DayOfWeek = _dayNames[s.DayOfWeek],
                DayNumber = s.DayOfWeek,
                LessonNumber = s.LessonHour?.OrderNumber ?? 0,
                StartTime = s.LessonHour?.StartTime.ToString(@"hh\:mm") ?? "",
                EndTime = s.LessonHour?.EndTime.ToString(@"hh\:mm") ?? "",
                SubjectName = s.Subject?.Name ?? "",
                TeacherName = s.Teacher != null ? $"{s.Teacher.FirstName} {s.Teacher.LastName}" : "",
                ClassroomName = s.Classroom?.Name ?? ""
            }).ToList();

            return (className, rows);
        }

        public async Task<byte[]> ExportScheduleToPdfAsync(int classId, int? yearId, int? semesterId)
        {
            var (className, rows) = await GetScheduleDataAsync(classId, yearId, semesterId);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(1, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text($"Plan lekcji - klasa {className}")
                        .SemiBold()
                        .FontSize(16)
                        .AlignCenter();

                    page.Content()
                        .PaddingVertical(10)
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);
                                columns.ConstantColumn(60);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.ConstantColumn(60);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text("Nr");
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text("Godziny");
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text("Przedmiot");
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text("Nauczyciel");
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(4).Text("Sala");
                            });

                            for (int day = 1; day <= 5; day++)
                            {
                                var dayRows = rows.Where(r => r.DayNumber == day).OrderBy(r => r.LessonNumber).ToList();
                                if (dayRows.Any())
                                {
                                    table.Cell().ColumnSpan(5).Background(Colors.Blue.Lighten4).Padding(4).Text(_dayNames[day]).SemiBold();

                                    foreach (var row in dayRows)
                                    {
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(row.LessonNumber.ToString());
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(4).Text($"{row.StartTime}-{row.EndTime}");
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(row.SubjectName);
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(row.TeacherName);
                                        table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(row.ClassroomName);
                                    }
                                }
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Wygenerowano: ");
                            x.Span(DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
                        });
                });
            });

            return document.GeneratePdf();
        }

        public async Task<byte[]> ExportScheduleToXlsxAsync(int classId, int? yearId, int? semesterId)
        {
            var (className, rows) = await GetScheduleDataAsync(classId, yearId, semesterId);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add($"Plan {className}");

            worksheet.Cell(1, 1).Value = "Dzień";
            worksheet.Cell(1, 2).Value = "Nr";
            worksheet.Cell(1, 3).Value = "Godziny";
            worksheet.Cell(1, 4).Value = "Przedmiot";
            worksheet.Cell(1, 5).Value = "Nauczyciel";
            worksheet.Cell(1, 6).Value = "Sala";

            var headerRange = worksheet.Range(1, 1, 1, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            var sortedRows = rows.OrderBy(r => r.DayNumber).ThenBy(r => r.LessonNumber).ToList();
            for (int i = 0; i < sortedRows.Count; i++)
            {
                var row = sortedRows[i];
                worksheet.Cell(i + 2, 1).Value = row.DayOfWeek;
                worksheet.Cell(i + 2, 2).Value = row.LessonNumber;
                worksheet.Cell(i + 2, 3).Value = $"{row.StartTime}-{row.EndTime}";
                worksheet.Cell(i + 2, 4).Value = row.SubjectName;
                worksheet.Cell(i + 2, 5).Value = row.TeacherName;
                worksheet.Cell(i + 2, 6).Value = row.ClassroomName;
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public async Task<byte[]> ExportScheduleToCsvAsync(int classId, int? yearId, int? semesterId)
        {
            var (className, rows) = await GetScheduleDataAsync(classId, yearId, semesterId);

            var sb = new StringBuilder();
            sb.AppendLine("Dzień;Nr;Godziny;Przedmiot;Nauczyciel;Sala");

            var sortedRows = rows.OrderBy(r => r.DayNumber).ThenBy(r => r.LessonNumber);
            foreach (var row in sortedRows)
            {
                sb.AppendLine($"{row.DayOfWeek};{row.LessonNumber};{row.StartTime}-{row.EndTime};{row.SubjectName};{row.TeacherName};{row.ClassroomName}");
            }

            return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
        }

        private class ScheduleRow
        {
            public string DayOfWeek { get; set; } = "";
            public int DayNumber { get; set; }
            public int LessonNumber { get; set; }
            public string StartTime { get; set; } = "";
            public string EndTime { get; set; } = "";
            public string SubjectName { get; set; } = "";
            public string TeacherName { get; set; } = "";
            public string ClassroomName { get; set; } = "";
        }
    }
}
