namespace API.DTOs
{
    public class ParentStudentDto
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string ParentName { get; set; } = string.Empty;
        public string ParentEmail { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
    }
}