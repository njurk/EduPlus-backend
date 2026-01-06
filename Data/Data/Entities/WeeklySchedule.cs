using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class WeeklySchedule
    {
        [Key]
        public int Id { get; set; }
        public int SchoolYearId { get; set; }
        [ForeignKey(nameof(SchoolYearId))]
        public virtual SchoolYear? SchoolYear { get; set; } = null!;
        public int SemesterId { get; set; }
        [ForeignKey(nameof(SemesterId))]
        public virtual Semester? Semester { get; set; } = null!;
        public int ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public virtual Class? Class { get; set; } = null!;
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public virtual Subject? Subject { get; set; } = null!;
        public int TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual User? Teacher { get; set; } = null!;
        public int ClassroomId { get; set; }
        [ForeignKey(nameof(ClassroomId))]
        public virtual Classroom? Classroom { get; set; } = null!;
        public int DayOfWeek { get; set; }
        public int LessonHourId { get; set; }
        [ForeignKey(nameof(LessonHourId))]
        public virtual LessonHour? LessonHour { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
