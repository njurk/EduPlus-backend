using System.ComponentModel.DataAnnotations;

namespace Data.Data.Entities
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public required string Subject { get; set; }
        public string Status { get; set; } = "Open";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public virtual ICollection<TicketMessage> Messages { get; set; } = new List<TicketMessage>();
    }
}