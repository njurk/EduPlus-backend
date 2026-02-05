namespace Shared.DTOs
{
    public class ExcuseAttendanceItemDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int LessonHour { get; set; }
    }
}
