namespace Shared.DTOs.Mobile
{
    public class MobileExcuseAttendanceDto
    {
        public int Id { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public int LessonHour { get; set; }
        public string AttendanceType { get; set; } = string.Empty;
        public string AttendanceTypeColorHex { get; set; }
    }
}
