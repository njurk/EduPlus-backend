using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class BehaviorNote
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]

        [Required]
        public virtual User Student { get; set; } = null!;

        public int TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual User Teacher { get; set; } = null!;

        public int BehaviorNoteTypeId { get; set; }
        [ForeignKey(nameof(BehaviorNoteTypeId))]

        [Required]
        public virtual BehaviorNoteType BehaviorNoteType { get; set; } = null!;

        [Required]
        public int SemesterId { get; set; }
        [ForeignKey(nameof(SemesterId))]

        [Required]
        public virtual Semester Semester { get; set; } = null!;

        [Required, MaxLength(255)]
        public required string Description { get; set; }

        [Required]
        public int Points { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}
