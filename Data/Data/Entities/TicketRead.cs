using Data.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Data.Entities
{
    [Index(nameof(TicketId), nameof(UserId), IsUnique = true)]
    public class TicketRead : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public int TicketId { get; set; }
        [ForeignKey(nameof(TicketId))]
        public virtual Ticket? Ticket { get; set; } = null!;
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? ModifiedByUserId { get; set; }
    }
}
