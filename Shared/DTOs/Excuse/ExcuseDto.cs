namespace Shared.DTOs
{
    public class ExcuseDto
    {
        public int Id { get; set; }
        public string ParentName { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public string? ClassName { get; set; }
        public bool? IsAccepted { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public string? ModifiedByName { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int AttendanceCount { get; set; }
    }

    public class ExcuseDetailsDto : ExcuseDto
    {
        public List<ExcuseAttendanceItemDto> Attendances { get; set; } = new();
    }

    public class ExcuseAttendanceItemDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int LessonHour { get; set; }
    }

    public class CreateExcuseDto
    {
        public int StudentId { get; set; }
        public List<int> AttendanceIds { get; set; } = new();
        public string Reason { get; set; } = string.Empty;
    }

    public class AcceptExcuseDto
    {
        public bool? IsAccepted { get; set; }
    }
}

