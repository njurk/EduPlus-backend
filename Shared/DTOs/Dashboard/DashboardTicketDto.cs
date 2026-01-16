namespace Shared.DTOs
{
    public class DashboardTicketDto
    {
        public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
        public bool IsClosed { get; set; }
    }
}
