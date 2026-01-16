using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? ModifiedByUserId { get; set; }
    }
}
