namespace Shared.DTOs
{
    public class ScheduleTemplateDto
    {
        public int Id { get; set; }
        public string SubjectName { get; set; } = "";
        public string ClassName { get; set; } = "";
        public string TeacherName { get; set; } = "";
        public string ClassroomName { get; set; } = "";
        public int OrderNumber { get; set; }
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
    }
}
