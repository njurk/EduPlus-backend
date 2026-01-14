namespace Shared.DTOs
{
    public class TicketDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string Subject { get; set; }
        public bool IsClosed { get; set; }
        public DateTime? ClosedAt { get; set; }
        public string? AdminResponse { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
