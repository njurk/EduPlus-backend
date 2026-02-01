using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Data.Entities
{
    public class ExcuseAttendance
    {
        [Key]
        public int Id { get; set; }
        public int ExcuseId { get; set; }
        [ForeignKey(nameof(ExcuseId))]
        public virtual Excuse? Excuse { get; set; } = null!;
        public int AttendanceId { get; set; }
        [ForeignKey(nameof(AttendanceId))]
        public virtual Attendance? Attendance { get; set; } = null!;
    }
}
