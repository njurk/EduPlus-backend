using BusinessLogic.Helpers;
using Data.Data;

namespace BusinessLogic.Services
{
    public interface IExportService
    {
        Task<ExportResult> ExportScheduleAsync(int classId, int? yearId, int? semesterId, string format);
        Task<ExportResult> ExportTeacherScheduleAsync(int teacherId, int semesterId, string format);
        Task<ExportResult> ExportGradesAsync(int classId, int subjectId, int semesterId, int schoolYearId, int? studentId, string format);
        Task<ExportResult> ExportAttendanceAsync(int classId, int semesterId, int schoolYearId, int? studentId, string format);
    }

    public class ExportResult
    {
        public byte[] Data { get; set; } = Array.Empty<byte>();
        public string FileName { get; set; } = "";
        public string ContentType { get; set; } = "";
    }

    public class ExportService : IExportService
    {
        private readonly ScheduleExportHelper _scheduleHelper;
        private readonly GradesExportHelper _gradesHelper;
        private readonly AttendanceExportHelper _attendanceHelper;

        private static readonly Dictionary<string, string> ContentTypes = new()
        {
            ["pdf"] = "application/pdf",
            ["xlsx"] = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ["docx"] = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        };

        public ExportService(EduPlusDbContext context)
        {
            _scheduleHelper = new ScheduleExportHelper(context);
            _gradesHelper = new GradesExportHelper(context);
            _attendanceHelper = new AttendanceExportHelper(context);
        }

        public async Task<ExportResult> ExportScheduleAsync(int classId, int? yearId, int? semesterId, string format)
        {
            var grid = await _scheduleHelper.GetScheduleGridAsync(classId, yearId, semesterId);
            var data = format switch
            {
                "pdf" => _scheduleHelper.GeneratePdf(grid),
                "xlsx" => _scheduleHelper.GenerateXlsx(grid),
                "docx" => _scheduleHelper.GenerateDocx(grid),
                _ => throw new ArgumentException($"Nieobsługiwany format: {format}")
            };

            return new ExportResult
            {
                Data = data,
                FileName = _scheduleHelper.GenerateFileName(grid.ClassName, format),
                ContentType = ContentTypes.GetValueOrDefault(format, "application/octet-stream")
            };
        }

        public async Task<ExportResult> ExportTeacherScheduleAsync(int teacherId, int semesterId, string format)
        {
            var grid = await _scheduleHelper.GetTeacherScheduleGridAsync(teacherId, semesterId);
            var data = format switch
            {
                "pdf" => _scheduleHelper.GeneratePdf(grid),
                "xlsx" => _scheduleHelper.GenerateXlsx(grid),
                "docx" => _scheduleHelper.GenerateDocx(grid),
                _ => throw new ArgumentException($"Nieobsługiwany format: {format}")
            };

            return new ExportResult
            {
                Data = data,
                FileName = _scheduleHelper.GenerateFileName(grid.ClassName, format),
                ContentType = ContentTypes.GetValueOrDefault(format, "application/octet-stream")
            };
        }

        public async Task<ExportResult> ExportGradesAsync(int classId, int subjectId, int semesterId, int schoolYearId, int? studentId, string format)
        {
            if (studentId.HasValue)
            {
                var studentData = await _gradesHelper.GetStudentGradesDataAsync(studentId.Value, classId, semesterId, schoolYearId);
                var studentBytes = format switch
                {
                    "pdf" => _gradesHelper.GenerateStudentPdf(studentData),
                    "xlsx" => _gradesHelper.GenerateStudentXlsx(studentData),
                    _ => throw new ArgumentException($"Nieobsługiwany format: {format}")
                };
                return new ExportResult
                {
                    Data = studentBytes,
                    FileName = _gradesHelper.GenerateStudentFileName(studentData, format),
                    ContentType = ContentTypes.GetValueOrDefault(format, "application/octet-stream")
                };
            }

            var data = await _gradesHelper.GetGradesDataAsync(classId, subjectId, semesterId, schoolYearId);
            var bytes = format switch
            {
                "pdf" => _gradesHelper.GeneratePdf(data),
                "xlsx" => _gradesHelper.GenerateXlsx(data),
                _ => throw new ArgumentException($"Nieobsługiwany format: {format}")
            };

            return new ExportResult
            {
                Data = bytes,
                FileName = _gradesHelper.GenerateFileName(data, format),
                ContentType = ContentTypes.GetValueOrDefault(format, "application/octet-stream")
            };
        }

        public async Task<ExportResult> ExportAttendanceAsync(int classId, int semesterId, int schoolYearId, int? studentId, string format)
        {
            if (studentId.HasValue)
            {
                var studentData = await _attendanceHelper.GetStudentAttendanceDataAsync(studentId.Value, classId, semesterId, schoolYearId);
                var studentBytes = format switch
                {
                    "pdf" => _attendanceHelper.GenerateStudentPdf(studentData),
                    "xlsx" => _attendanceHelper.GenerateStudentXlsx(studentData),
                    _ => throw new ArgumentException($"Nieobsługiwany format: {format}")
                };
                return new ExportResult
                {
                    Data = studentBytes,
                    FileName = _attendanceHelper.GenerateStudentFileName(studentData, format),
                    ContentType = ContentTypes.GetValueOrDefault(format, "application/octet-stream")
                };
            }

            var data = await _attendanceHelper.GetAttendanceDataAsync(classId, semesterId, schoolYearId);
            var bytes = format switch
            {
                "pdf" => _attendanceHelper.GeneratePdf(data),
                "xlsx" => _attendanceHelper.GenerateXlsx(data),
                _ => throw new ArgumentException($"Nieobsługiwany format: {format}")
            };

            return new ExportResult
            {
                Data = bytes,
                FileName = _attendanceHelper.GenerateFileName(data, format),
                ContentType = ContentTypes.GetValueOrDefault(format, "application/octet-stream")
            };
        }
    }
}
