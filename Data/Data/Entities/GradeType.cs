using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class GradeType
    {
        [Key]
        public int Id { get; set; }
        public required string Numeric { get; set; }

        [Column(TypeName = "decimal(2, 1)")]
        public required decimal Value { get; set; }
        public required string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
