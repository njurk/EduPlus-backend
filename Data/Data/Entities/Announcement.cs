using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class Announcement
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
        public virtual User Author { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
