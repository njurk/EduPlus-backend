namespace Shared.DTOs
{
    public class BulkGradeCreateDto
    {
        public int SubjectId { get; set; }
        public int GradeCategoryId { get; set; }
        public int? GradeColumnId { get; set; }
        public List<BulkGradeItem> Grades { get; set; } = new();
    }
}
