using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class AnnouncementTarget
    {
        [Key]
        public int Id { get; set; }

        public int AnnouncementId { get; set; }
        [ForeignKey(nameof(AnnouncementId))]
        public virtual Announcement Announcement { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public int? ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public virtual Class? Class { get; set; }
    }
}
