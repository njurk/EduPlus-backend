using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Data.Entities
{
    [Index(nameof(AnnouncementId), nameof(RoleId), IsUnique = true)]
    public class AnnouncementTarget
    {
        [Key]
        public int Id { get; set; }

        public int AnnouncementId { get; set; }
        [ForeignKey(nameof(AnnouncementId))]
        public virtual Announcement? Announcement { get; set; }

        public int? RoleId { get; set; }
        [ForeignKey(nameof(RoleId))]
        public virtual Role? Role { get; set; }
    }
}
