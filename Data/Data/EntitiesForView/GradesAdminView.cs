using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Data.EntitiesForView
{
    public class GradesAdminView
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int GradeTypeId { get; set; }
        public int GradeCategoryId { get; set; }
        public int TeacherId { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string? ClassName { get; set; }
        public int? ClassId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string GradeTypeName { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal GradeValue { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryColorHex { get; set; } = "#6b7280";
        public string TeacherName { get; set; } = string.Empty;
        public string? ModifiedByName { get; set; }
    }
}
