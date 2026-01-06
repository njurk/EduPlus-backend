using System.ComponentModel.DataAnnotations;

namespace Data.Data.Entities
{
    public class TicketMessage
    {
        [Key]
        public int Id { get; set; }
        public int TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
        public int SenderId { get; set; }
        public virtual User Sender { get; set; }
        public required string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}