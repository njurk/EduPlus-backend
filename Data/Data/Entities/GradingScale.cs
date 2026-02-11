using Data.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Data.Entities
{
    public class GradingScale : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public int GradeTypeId { get; set; }
        [ForeignKey(nameof(GradeTypeId))]
        public virtual GradeType? GradeType { get; set; } = null!;
        [Column(TypeName = "decimal(4, 2)")]
        public decimal MinAverage { get; set; }
        [Column(TypeName = "decimal(4, 2)")]
        public decimal MaxAverage { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? ModifiedByUserId { get; set; }
    }
}
