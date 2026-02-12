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
using System.Threading.Tasks;

namespace BusinessLogic.Helpers
{
    public class ScheduleExportHelper
    {
        private readonly EduPlusDbContext _context;
        private readonly string[] _dayNamesFull = { "", "Poniedziałek", "Wtorek", "Środa", "Czwartek", "Piątek" };

        public ScheduleExportHelper(EduPlusDbContext context)
        {
            _context = context;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<ScheduleGrid> GetScheduleGridAsync(int classId, int? yearId, int? semesterId)
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

        public async Task<ScheduleGrid> GetTeacherScheduleGridAsync(int teacherId, int semesterId)
        {
            var teacher = await _context.Users.FirstOrDefaultAsync(u => u.Id == teacherId);
            var teacherLabel = teacher != null ? $"{teacher.FirstName} {teacher.LastName}" : "Nauczyciel";

            var semester = await _context.Semesters.Include(s => s.SchoolYear).FirstOrDefaultAsync(s => s.Id == semesterId);
            var yearName = semester?.SchoolYear?.Name;
            var semesterName = semester?.Name;

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

            var schedules = await _context.WeeklySchedules
                .Where(ws => ws.TeacherId == teacherId && ws.SemesterId == semesterId && ws.IsActive)
                .Include(ws => ws.Subject)
                .Include(ws => ws.Class)
                .Include(ws => ws.Classroom)
                .Include(ws => ws.LessonHour)
                .ToListAsync();

            var cells = schedules.ToDictionary(
                s => (s.LessonHour?.OrderNumber ?? 0, s.DayOfWeek),
                s => new ScheduleCell
                {
                    SubjectName = s.Subject?.Name ?? "",
                    TeacherName = s.Class != null ? $"{s.Class.Level}{s.Class.Letter}" : "",
                    ClassroomName = s.Classroom?.Name ?? ""
                }
            );

            return new ScheduleGrid
            {
                ClassName = teacherLabel,
                IsTeacher = true,
                YearName = yearName,
                SemesterName = semesterName,
                LessonHours = lessonHours,
                Cells = cells
            };
        }

        public string GenerateFileName(string className, string extension) =>
            $"{DateTime.Now:yyyyMMddHHmmss}-plan-lekcji-{className.ToLower().Replace(" ", "-")}.{extension}";

        public string GetMainHeader(ScheduleGrid grid)
        {
            var parts = new List<string> { "Plan lekcji" };
            if (!string.IsNullOrEmpty(grid.YearName)) parts.Add(grid.YearName);
            if (!string.IsNullOrEmpty(grid.SemesterName)) parts.Add(grid.SemesterName.ToLower());
            parts.Add(grid.IsTeacher ? grid.ClassName : $"klasa {grid.ClassName}");
            return string.Join(", ", parts);
        }

        public string GetCellText(ScheduleCell? cell) =>
            cell != null && !string.IsNullOrEmpty(cell.SubjectName)
                ? $"{cell.SubjectName}\n{cell.ClassroomName}\n{cell.TeacherName}"
                : "";

        public byte[] GeneratePdf(ScheduleGrid grid)
        {
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

                        table.Header(h =>
                        {
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text("Nr").FontSize(9).SemiBold();
                            h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text("Godz.").FontSize(9).SemiBold();
                            for (int d = 1; d <= 5; d++)
                                h.Cell().Background(Colors.Grey.Lighten2).Border(1).BorderColor(Colors.Grey.Lighten1).Padding(4).AlignCenter().Text(_dayNamesFull[d]).FontSize(9).SemiBold();
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

            return document.GeneratePdf();
        }

        public byte[] GenerateXlsx(ScheduleGrid grid)
        {
            var header = GetMainHeader(grid);

            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add($"Plan {grid.ClassName}");

            int row = 1;
            ws.Cell(row, 1).Value = header;
            ws.Range(row, 1, row, 7).Merge().Style.Font.SetBold().Font.FontSize = 14;
            row += 2;

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
            return stream.ToArray();
        }

        public byte[] GenerateDocx(ScheduleGrid grid)
        {
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

            return stream.ToArray();
        }

        private Paragraph CreateParagraph(string text, bool bold, string fontSize, JustificationValues justification)
        {
            var runProps = new RunProperties(new FontSize { Val = fontSize });
            if (bold) runProps.Append(new Bold());
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
    }

    public class ScheduleGrid
    {
        public string ClassName { get; set; } = "";
        public bool IsTeacher { get; set; }
        public string? YearName { get; set; }
        public string? SemesterName { get; set; }
        public List<LessonHourInfo> LessonHours { get; set; } = new();
        public Dictionary<(int order, int day), ScheduleCell> Cells { get; set; } = new();
    }

    public class LessonHourInfo
    {
        public int Order { get; set; }
        public string Start { get; set; } = "";
        public string End { get; set; } = "";
    }

    public class ScheduleCell
    {
        public string SubjectName { get; set; } = "";
        public string TeacherName { get; set; } = "";
        public string ClassroomName { get; set; } = "";
    }
}
