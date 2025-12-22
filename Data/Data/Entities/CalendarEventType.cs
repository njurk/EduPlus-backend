using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class CalendarEventType
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string Name { get; set; }

        public int CalendarColorId { get; set; }
        [ForeignKey(nameof(CalendarColorId))]
        public virtual CalendarColor CalendarColor { get; set; } = null!;
    }
}
