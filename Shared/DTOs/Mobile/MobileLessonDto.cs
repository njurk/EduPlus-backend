namespace Shared.DTOs.Mobile
{
    public class MobileLessonDto
    {
        public int DayOfWeek { get; set; }
        public int OrderNumber { get; set; }
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
        public string SubjectName { get; set; } = "";
        public string TeacherName { get; set; } = "";
        public string ClassroomName { get; set; } = "";
    }
}
