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
        public bool IsClosed { get; set; } = false;
        public DateTime? ClosedAt { get; set; }
        public string? AdminResponse { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? ModifiedByUserId { get; set; }
    }
}
