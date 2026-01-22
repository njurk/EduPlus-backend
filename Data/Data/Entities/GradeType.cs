using Data.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Data.Entities
{
    public class GradeType : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Numeric { get; set; }

        [Column(TypeName = "decimal(2, 1)")]
        public required decimal Value { get; set; }
        public required string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? ModifiedByUserId { get; set; }
    }
}

