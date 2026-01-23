namespace Shared.DTOs
{
    public class WeeklyScheduleDto
    {
        public int? Id { get; set; }
        public int ClassId { get; set; }
        public int SemesterId { get; set; }
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public int ClassroomId { get; set; }
        public int DayOfWeek { get; set; }
        public int LessonHourId { get; set; }
    }
}
