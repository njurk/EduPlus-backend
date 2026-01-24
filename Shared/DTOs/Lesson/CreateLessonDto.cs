using System;

namespace Shared.DTOs
{
    public class CreateLessonDto
    {
        public int SubjectId { get; set; }
        public int TeacherId { get; set; }
        public int ClassId { get; set; }
        public int ClassroomId { get; set; }
        public int LessonHourId { get; set; }
        public string? Topic { get; set; }
        public int StatusId { get; set; }
        public DateTime Date { get; set; }
    }

    public class UpdateLessonDto
    {
        public string? Topic { get; set; }
        public int? StatusId { get; set; }
        public int? ClassroomId { get; set; }
        public int? TeacherId { get; set; }
    }

    public class CreateFromScheduleDto
    {
        public int ScheduleId { get; set; }
        public DateTime Date { get; set; }
        public int? TeacherId { get; set; }
        public int? StatusId { get; set; }
    }
}
