using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class ParentStudent
    {
        [Key]
        public int Id { get; set; }
        public int ParentId { get; set; }
        [ForeignKey(nameof(ParentId))]
        public virtual User? Parent { get; set; } = null!;
        public int StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public virtual User? Student { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
