using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class ClassStudent
    {
        [Key]
        public int Id { get; set; }
        public int ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public virtual Class? Class { get; set; } = null!;
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual User? Student { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
