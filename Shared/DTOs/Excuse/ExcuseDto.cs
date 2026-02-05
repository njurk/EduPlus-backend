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
}
