namespace Shared.DTOs.Mobile
{
    public class MobileNegativeAttendanceDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string SubjectName { get; set; } = "";
        public int LessonHour { get; set; }
        public string AttendanceType { get; set; } = "";
        public string AttendanceTypeColorHex { get; set; } = "";
    }
}
