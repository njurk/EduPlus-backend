using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Data.Entities
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual User? Student { get; set; } = null!;
        public int TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual User? Teacher { get; set; } = null!;
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public virtual Subject? Subject { get; set; } = null!;
        public int GradeTypeId { get; set; }
        [ForeignKey(nameof(GradeTypeId))]
        public virtual GradeType? GradeType { get; set; } = null!;
        public int GradeCategoryId { get; set; }
        [ForeignKey(nameof(GradeCategoryId))]
        public virtual GradeCategory? GradeCategory { get; set; } = null!;
        public DateTime DateTime { get; set; } = DateTime.Now;
        [MaxLength(255)]
        public string? Comment { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
