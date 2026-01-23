using ClosedXML.Excel;
using Data.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
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
        Task<ExportResult> ExportScheduleToPdfAsync(int classId, int? yearId, int? semesterId);
        Task<ExportResult> ExportScheduleToXlsxAsync(int classId, int? yearId, int? semesterId);
        Task<ExportResult> ExportScheduleToCsvAsync(int classId, int? yearId, int? semesterId);
        Task<ExportResult> ExportScheduleToDocxAsync(int classId, int? yearId, int? semesterId);
    }

    public class ExportResult
    {
        public byte[] Data { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = "";
    }

    public class ExportService : IExportService
    {
        private readonly EduPlusDbContext _context;
        private readonly string[] _dayNamesFull = { "", "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek" };

        public ExportService(EduPlusDbContext context)
        {
            _context = context;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        private async Task<ScheduleGrid> GetScheduleGridAsync(int classId, int? yearId, int? semesterId)
        {
            var classEntity = await _context.Classes.FirstOrDefaultAsync(c => c.Id == classId);
            var className = classEntity != null ? $"{classEntity.Level}{classEntity.Letter}" : "Klasa";

            string? yearName = null, semesterName = null;

            if (yearId.HasValue)
                yearName = (await _context.SchoolYears.FirstOrDefaultAsync(y => y.Id == yearId.Value))?.Name;

            if (semesterId.HasValue)
                semesterName = (await _context.Semesters.FirstOrDefaultAsync(s => s.Id == semesterId.Value))?.Name;

            var lessonHours = await _context.LessonHours
                .Where(lh => lh.IsActive)
                .OrderBy(lh => lh.OrderNumber)
                .Select(lh => new LessonHourInfo
                {
                    Order = lh.OrderNumber,
                    Start = lh.StartTime.ToString(@"HH\:mm"),
                    End = lh.EndTime.ToString(@"HH\:mm")
                })
                .ToListAsync();

            var query = _context.WeeklySchedules
                .Where(ws => ws.ClassId == classId && ws.IsActive)
                .Include(ws => ws.Subject)
                .Include(ws => ws.Teacher)
                .Include(ws => ws.Classroom)
                .Include(ws => ws.LessonHour);

            IQueryable<Data.Data.Entities.WeeklySchedule> filteredQuery = query;
            if (yearId.HasValue) filteredQuery = filteredQuery.Where(ws => ws.SchoolYearId == yearId.Value);
            if (semesterId.HasValue) filteredQuery = filteredQuery.Where(ws => ws.SemesterId == semesterId.Value);

            var schedules = await filteredQuery.ToListAsync();
            var cells = schedules.ToDictionary(
                s => (s.LessonHour?.OrderNumber ?? 0, s.DayOfWeek),
                s => new ScheduleCell
                {
                    SubjectName = s.Subject?.Name ?? "",
                    TeacherName = s.Teacher != null ? $"{s.Teacher.FirstName[0]}. {s.Teacher.LastName}" : "",
                    ClassroomName = s.Classroom?.Name ?? ""
                }
            );

            return new ScheduleGrid
            {
                ClassName = className,
                YearName = yearName,
                SemesterName = semesterName,
                LessonHours = lessonHours,
                Cells = cells
            };
        }

        private string GenerateFileName(string className, string extension) =>
            $"{DateTime.Now:yyyyMMddHHmmss}-plan-lekcji-{className.ToLower().Replace(" ", "-")}.{extension}";

        private string GetMainHeader(ScheduleGrid grid)
        {
            var parts = new List<string> { "Plan lekcji" };
            if (!string.IsNullOrEmpty(grid.YearName)) parts.Add(grid.YearName);
            if (!string.IsNullOrEmpty(grid.SemesterName)) parts.Add(grid.SemesterName.ToLower());
            parts.Add($"klasa {grid.ClassName}");
            return string.Join(", ", parts);
        }

        private string GetCellText(ScheduleCell? cell) =>
            cell != null && !string.IsNullOrEmpty(cell.SubjectName)
                ? $"{cell.SubjectName}\n{cell.ClassroomName}\n{cell.TeacherName}"
                : "";

        public async Task<ExportResult> ExportScheduleToPdfAsync(int classId, int? yearId, int? semesterId)
        {
            var grid = await GetScheduleGridAsync(classId, yearId, semesterId);
            var header = GetMainHeader(grid);

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
                        .FontSize(16);

                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(25);
                            columns.ConstantColumn(50);
                            for (int i = 0; i < 5; i++) columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text("Nr").FontSize(9).SemiBold();
                            header.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text("Godz.").FontSize(9).SemiBold();
                            for (int d = 1; d <= 5; d++)
                                header.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(_dayNamesFull[d]).FontSize(9).SemiBold();
                        });

                        foreach (var hour in grid.LessonHours)
                        {
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Background(Colors.Grey.Lighten3).Padding(3).AlignCenter().Text(hour.Order.ToString()).FontSize(9).SemiBold();
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Background(Colors.Grey.Lighten3).Padding(3).AlignCenter().Text($"{hour.Start}\n{hour.End}").FontSize(8);

                            for (int d = 1; d <= 5; d++)
                            {
                                var cell = grid.Cells.GetValueOrDefault((hour.Order, d));
                                var cellContent = table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).MinHeight(45);

                                if (cell != null && !string.IsNullOrEmpty(cell.SubjectName))
                                {
                                    cellContent.Column(col =>
                                    {
                                        col.Item().Text(cell.SubjectName).FontSize(9).SemiBold();
                                        if (!string.IsNullOrEmpty(cell.ClassroomName))
                                            col.Item().Text(cell.ClassroomName).FontSize(8).FontColor(Colors.Grey.Darken1);
                                        col.Item().Text(cell.TeacherName).FontSize(8).FontColor(Colors.Grey.Darken2);
                                    });
                                }
                                else
                                {
                                    cellContent.AlignCenter().AlignMiddle().Text("").FontSize(9).FontColor(Colors.Grey.Lighten1);
                                }
                            }
                        }
                    });

                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.Span("Wygenerowano: ").FontSize(8);
                        x.Span(DateTime.Now.ToString("dd.MM.yyyy HH:mm")).FontSize(8);
                    });
                });
            });

            return new ExportResult
            {
                Data = document.GeneratePdf(),
                FileName = GenerateFileName(grid.ClassName, "pdf")
            };
        }

        public async Task<ExportResult> ExportScheduleToXlsxAsync(int classId, int? yearId, int? semesterId)
        {
            var grid = await GetScheduleGridAsync(classId, yearId, semesterId);
            var header = GetMainHeader(grid);

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add($"Plan {grid.ClassName}");

            int row = 1;
            
            ws.Cell(row, 1).Value = header;
            ws.Range(row, 1, row, 7).Merge().Style.Font.SetBold().Font.FontSize = 14;
            row++;
            row++;

            ws.Cell(row, 1).Value = "Nr";
            ws.Cell(row, 2).Value = "Godziny";
            for (int d = 1; d <= 5; d++)
                ws.Cell(row, d + 2).Value = _dayNamesFull[d];

            ws.Range(row, 1, row, 7).Style.Font.SetBold().Fill.BackgroundColor = XLColor.LightGray;
            ws.Range(row, 1, row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            row++;

            foreach (var hour in grid.LessonHours)
            {
                ws.Cell(row, 1).Value = hour.Order;
                ws.Cell(row, 2).Value = $"{hour.Start}-{hour.End}";

                for (int d = 1; d <= 5; d++)
                {
                    var cell = grid.Cells.GetValueOrDefault((hour.Order, d));
                    ws.Cell(row, d + 2).Value = GetCellText(cell);
                    ws.Cell(row, d + 2).Style.Alignment.WrapText = true;
                }
                row++;
            }

            ws.Column(1).Width = 5;
            ws.Column(2).Width = 12;
            for (int d = 3; d <= 7; d++) ws.Column(d).Width = 20;
            ws.Rows().Height = 40;
            ws.Row(1).Height = 20;

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return new ExportResult { Data = stream.ToArray(), FileName = GenerateFileName(grid.ClassName, "xlsx") };
        }

        public async Task<ExportResult> ExportScheduleToCsvAsync(int classId, int? yearId, int? semesterId)
        {
            var grid = await GetScheduleGridAsync(classId, yearId, semesterId);
            var header = GetMainHeader(grid);

            var sb = new StringBuilder();
            sb.AppendLine(header);
            sb.AppendLine();

            sb.AppendLine($"Nr;Godziny;{string.Join(";", _dayNamesFull.Skip(1))}");

            foreach (var hour in grid.LessonHours)
            {
                var row = new List<string> { hour.Order.ToString(), $"{hour.Start}-{hour.End}" };
                for (int d = 1; d <= 5; d++)
                {
                    var cell = grid.Cells.GetValueOrDefault((hour.Order, d));
                    row.Add(cell != null && !string.IsNullOrEmpty(cell.SubjectName)
                        ? $"\"{cell.SubjectName}\n{cell.ClassroomName}\n{cell.TeacherName}\""
                        : "");
                }
                sb.AppendLine(string.Join(";", row));
            }

            return new ExportResult
            {
                Data = Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray(),
                FileName = GenerateFileName(grid.ClassName, "csv")
            };
        }

        public async Task<ExportResult> ExportScheduleToDocxAsync(int classId, int? yearId, int? semesterId)
        {
            var grid = await GetScheduleGridAsync(classId, yearId, semesterId);
            var header = GetMainHeader(grid);

            using var stream = new MemoryStream();
            using (var doc = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document, true))
            {
                var mainPart = doc.AddMainDocumentPart();
                mainPart.Document = new DocumentFormat.OpenXml.Wordprocessing.Document(new Body());
                var body = mainPart.Document.Body!;

                body.Append(CreateParagraph(header, true, "32", JustificationValues.Center));
                body.Append(new Paragraph());

                var table = CreateDocxTable();
                var headerRow = new TableRow();
                headerRow.Append(CreateCell("Nr", true));
                headerRow.Append(CreateCell("Godziny", true));
                for (int d = 1; d <= 5; d++) headerRow.Append(CreateCell(_dayNamesFull[d], true));
                table.Append(headerRow);

                foreach (var hour in grid.LessonHours)
                {
                    var tr = new TableRow();
                    tr.Append(CreateCell(hour.Order.ToString(), false));
                    tr.Append(CreateCell($"{hour.Start}-{hour.End}", false));
                    for (int d = 1; d <= 5; d++)
                        tr.Append(CreateCell(GetCellText(grid.Cells.GetValueOrDefault((hour.Order, d))), false));
                    table.Append(tr);
                }

                body.Append(table);
                body.Append(new Paragraph());
                body.Append(CreateParagraph($"Wygenerowano: {DateTime.Now:dd.MM.yyyy HH:mm}", false, "18", JustificationValues.Center));

                var sectionProps = new SectionProperties(
                    new DocumentFormat.OpenXml.Wordprocessing.PageSize { Orient = PageOrientationValues.Landscape, Width = 15840, Height = 12240 },
                    new PageMargin { Top = 720, Right = 720, Bottom = 720, Left = 720 }
                );
                body.Append(sectionProps);
            }

            return new ExportResult { Data = stream.ToArray(), FileName = GenerateFileName(grid.ClassName, "docx") };
        }

        private Paragraph CreateParagraph(string text, bool bold, string fontSize, JustificationValues justification, bool italic = false)
        {
            var runProps = new RunProperties(new FontSize { Val = fontSize });
            if (bold) runProps.Append(new Bold());
            if (italic) runProps.Append(new Italic());

            return new Paragraph(
                new ParagraphProperties(new Justification { Val = justification }),
                new Run(runProps, new Text(text))
            );
        }

        private Table CreateDocxTable() => new Table(
            new TableProperties(
                new TableBorders(
                    new TopBorder { Val = BorderValues.Single, Size = 4 },
                    new BottomBorder { Val = BorderValues.Single, Size = 4 },
                    new LeftBorder { Val = BorderValues.Single, Size = 4 },
                    new RightBorder { Val = BorderValues.Single, Size = 4 },
                    new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                    new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                ),
                new TableWidth { Width = "5000", Type = TableWidthUnitValues.Pct }
            )
        );

        private TableCell CreateCell(string text, bool isHeader)
        {
            var cellProps = new TableCellProperties();
            if (isHeader)
                cellProps.Append(new Shading { Fill = "D3D3D3" });
            
            if (text == "-")
                cellProps.Append(new TableCellVerticalAlignment { Val = TableVerticalAlignmentValues.Center });

            var cell = new TableCell(cellProps);

            var runProps = new RunProperties(new FontSize { Val = "20" });
            if (isHeader) runProps.Append(new Bold());

            var paraProps = new ParagraphProperties();
            if (text == "-")
                paraProps.Append(new Justification { Val = JustificationValues.Center });

            var run = new Run(runProps);
            var lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                run.Append(new Text(lines[i]) { Space = SpaceProcessingModeValues.Preserve });
                if (i < lines.Length - 1) run.Append(new Break());
            }

            cell.Append(new Paragraph(paraProps, run));
            return cell;
        }

        private class ScheduleGrid
        {
            public string ClassName { get; set; } = "";
            public string? YearName { get; set; }
            public string? SemesterName { get; set; }
            public List<LessonHourInfo> LessonHours { get; set; } = new();
            public Dictionary<(int order, int day), ScheduleCell> Cells { get; set; } = new();
        }

        private class LessonHourInfo
        {
            public int Order { get; set; }
            public string Start { get; set; } = "";
            public string End { get; set; } = "";
        }

        private class ScheduleCell
        {
            public string SubjectName { get; set; } = "";
            public string TeacherName { get; set; } = "";
            public string ClassroomName { get; set; } = "";
        }
    }
}
