using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class CalendarEvent
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public required string Title { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        public DateTime StartDateTime { get; set; }

        public int CalendarEventTypeId { get; set; }
        [ForeignKey(nameof(CalendarEventTypeId))]
        public virtual CalendarEventType CalendarEventType { get; set; } = null!;

        // jeśli null - wydarzenie widoczne dla wszystkich
        // w przeciwnym razie wydarzenie widoczne tylko dla danej klasy
        public int? ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public virtual Class? Class { get; set; }
        public int? ClassSubjectId { get; set; }
        [ForeignKey(nameof(ClassSubjectId))]
        public virtual ClassSubject? ClassSubject { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;
    }
}
