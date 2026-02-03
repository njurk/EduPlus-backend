namespace Shared.DTOs.Mobile
{
    public class MobileScheduleDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = "";
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = "";
        public List<MobileLessonDto> Lessons { get; set; } = new();
    }

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
