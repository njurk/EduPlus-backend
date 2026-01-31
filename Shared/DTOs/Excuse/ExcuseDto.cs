namespace Shared.DTOs
{
    public class ExcuseDto
    {
        public int Id { get; set; }
        public string ParentName { get; set; } = string.Empty;
        public DateTime LessonDate { get; set; }
        public bool? IsAccepted { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public string? ModifiedByName { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateExcuseDto
    {
        public int AttendanceId { get; set; }
        public int ParentId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class AcceptExcuseDto
    {
        public bool IsAccepted { get; set; }
    }
}
