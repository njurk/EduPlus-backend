namespace Shared.DTOs
{
    public class DashboardTicketDto
    {
        public int Id { get; set; }
        public string ReasonName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
        public bool IsClosed { get; set; }
    }
}
