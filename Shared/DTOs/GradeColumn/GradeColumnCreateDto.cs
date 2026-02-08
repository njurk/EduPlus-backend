namespace Shared.DTOs
{
    public class GradeColumnCreateDto
    {
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public int SemesterId { get; set; }
        public int GradeCategoryId { get; set; }
        public string? Name { get; set; }
    }
}
