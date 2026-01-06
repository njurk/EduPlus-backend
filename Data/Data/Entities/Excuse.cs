using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class Excuse
    {
        [Key]
        public int Id { get; set; }
        public int AttendanceId { get; set; }
        [ForeignKey(nameof(AttendanceId))]
        public virtual Attendance? Attendance { get; set; } = null!;
        public int ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual User? Parent { get; set; } = null!;
        public required string Reason { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        public bool? IsAccepted { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
