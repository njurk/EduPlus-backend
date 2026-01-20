using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Data.Entities
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public required string Email { get; set; }
        public int ReasonId { get; set; }
        [ForeignKey(nameof(ReasonId))]
        public virtual TicketReason? Reason { get; set; }
        public required string Content { get; set; }
        public bool IsClosed { get; set; } = false;
        public DateTime? ClosedAt { get; set; }
        public string? AdminResponse { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? ModifiedByUserId { get; set; }
    }
}

