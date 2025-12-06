using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class TeacherClassSubject
    {
        [Key]
        public int Id { get; set; }

        public int TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual User Teacher { get; set; } = null!;

        public int ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public virtual Class Class { get; set; } = null!;

        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public virtual Subject Subject { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
