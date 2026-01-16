using System;

namespace Shared.DTOs
{
    public class ExcuseDto
    {
        public int Id { get; set; }
        public int AttendanceId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public DateTime LessonDate { get; set; }
        public int ParentId { get; set; }
        public string ParentName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public bool? IsAccepted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class CreateExcuseDto
    {
        public int AttendanceId { get; set; }
        public int ParentId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class UpdateExcuseDto
    {
        public string? Reason { get; set; }
    }

    public class AcceptExcuseDto
    {
        public bool IsAccepted { get; set; }
    }
}
