namespace Shared.DTOs
{
    public class CreateExcuseDto
    {
        public int StudentId { get; set; }
        public List<int> AttendanceIds { get; set; } = new();
        public string Reason { get; set; } = string.Empty;
    }
}
