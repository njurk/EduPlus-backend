namespace Shared.DTOs.Mobile
{
    public class MobileAttendanceDto
    {
        public List<MobileSubjectAttendanceDto> Subjects { get; set; } = new();
        public List<MobileAttendanceRecordDto> RecentRecords { get; set; } = new();
        public List<MobileDailyLessonDto> DailyLessons { get; set; } = new();
        public List<MobileAttendanceStatDto> Stats { get; set; } = new();
        public int TotalLessons { get; set; }
    }
}
