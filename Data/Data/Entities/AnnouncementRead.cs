using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    [Index(nameof(AnnouncementId), nameof(UserId), IsUnique = true)]
    public class AnnouncementRead
    {
        [Key]
        public int Id { get; set; }

        public int AnnouncementId { get; set; }
        [ForeignKey(nameof(AnnouncementId))]
        public virtual Announcement Announcement { get; set; } = null!;

        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        public DateTime ReadAt { get; set; } = DateTime.UtcNow;
    }
}
