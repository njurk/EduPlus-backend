using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class SubjectTeacher
    {
        public int SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public virtual Subject Subject { get; set; } = null!;

        public int TeacherId { get; set; }
        [ForeignKey(nameof(TeacherId))]
        public virtual User Teacher { get; set; } = null!;
    }
}
