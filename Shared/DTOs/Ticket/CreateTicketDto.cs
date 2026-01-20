namespace Shared.DTOs
{
    public class CreateTicketDto
    {
        public required string Email { get; set; }
        public int ReasonId { get; set; }
        public required string Content { get; set; }
    }
}
