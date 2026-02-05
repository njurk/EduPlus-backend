namespace Shared.DTOs.Mobile
{
    public class MobileDailyLessonDto
    {
        public int LessonOrder { get; set; }
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
        public string SubjectName { get; set; } = "";
        public string? AttendanceType { get; set; }
        public string? AttendanceTypeColorHex { get; set; }
    }
}
