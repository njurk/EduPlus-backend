using Data.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Data.Entities
{
    public class Attendance : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public int LessonId { get; set; }
        [ForeignKey(nameof(LessonId))]
        public virtual Lesson? Lesson { get; set; } = null!;
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual User? Student { get; set; } = null!;
        public int AttendanceTypeId { get; set; }
        [ForeignKey(nameof(AttendanceTypeId))]
        public virtual AttendanceType? AttendanceType { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? ModifiedByUserId { get; set; }
        [ForeignKey(nameof(ModifiedByUserId))]
        public virtual User? ModifiedBy { get; set; }
    }
}

