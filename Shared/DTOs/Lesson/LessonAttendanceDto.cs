namespace Shared.DTOs
{
    public class LessonAttendanceDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int StudentNumber { get; set; }
        public int? AttendanceTypeId { get; set; }
        public string AttendanceTypeName { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty;
    }
}
