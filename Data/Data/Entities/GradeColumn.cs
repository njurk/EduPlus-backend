using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Data.Entities
{
    public class GradeColumn
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(100)]
        public string? Name { get; set; }
        public int ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public virtual Class? Class { get; set; } = null!;
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public virtual Subject? Subject { get; set; } = null!;
        public int SemesterId { get; set; }
        [ForeignKey(nameof(SemesterId))]
        public virtual Semester? Semester { get; set; } = null!;
        public int GradeCategoryId { get; set; }
        [ForeignKey(nameof(GradeCategoryId))]
        public virtual GradeCategory? GradeCategory { get; set; } = null!;
        public int TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual User? Teacher { get; set; } = null!;
        public int Order { get; set; }
    }
}
