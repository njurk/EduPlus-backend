using Data.Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Data.Entities
{
    public class Announcement : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public required string Title { get; set; }

        [Required]
        public required string Description { get; set; }

        public bool IsActive { get; set; } = true;

        public int AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public virtual User? Author { get; set; } = null!;

        public virtual ICollection<AnnouncementTarget> AnnouncementTargets { get; set; } = new List<AnnouncementTarget>();

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? ModifiedByUserId { get; set; }
    }
}
