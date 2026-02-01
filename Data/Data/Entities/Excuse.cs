using Data.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Data.Entities
{
    public class Excuse : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual User? Student { get; set; } = null!;
        public int ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual User? Parent { get; set; } = null!;
        public required string Reason { get; set; }
        public bool? IsAccepted { get; set; }
        public DateTime? AcceptedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? ModifiedByUserId { get; set; }
        public virtual ICollection<ExcuseAttendance> ExcuseAttendances { get; set; } = new List<ExcuseAttendance>();
    }
}
