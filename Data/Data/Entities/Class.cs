using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Data.Entities
{
    public class Class
    {
        [Key]
        public int Id { get; set; }
        public required int Level { get; set; }
        [MaxLength(30)]
        public required string Letter { get; set; }
        public int SchoolYearId { get; set; }
        [ForeignKey(nameof(SchoolYearId))]
        public virtual SchoolYear? SchoolYear { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<ClassStudent> ClassStudents { get; set; } = new List<ClassStudent>();
    }
}
