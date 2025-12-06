using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class BehaviorGrade
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]

        public virtual User Student { get; set; } = null!;

        [Required]
        public int SemesterId { get; set; }
        [ForeignKey(nameof(SemesterId))]
        public virtual Semester Semester { get; set; } = null!;

        public int SchoolYearId { get; set; }
        [ForeignKey(nameof(SchoolYearId))]
        public virtual SchoolYear SchoolYear { get; set; } = null!;

        public int TotalPoints { get; set; }

        [Required, MaxLength(50)]
        public required string GradeName { get; set; }

        public int CalculatedFromRangeId { get; set; }
        [ForeignKey(nameof(CalculatedFromRangeId))]
        public virtual BehaviorGradeRange CalculatedFromRange { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}
