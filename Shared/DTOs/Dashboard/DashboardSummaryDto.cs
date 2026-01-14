namespace Shared.DTOs
{
    public class DashboardSummaryDto
    {
        public DashboardStatsDto Stats { get; set; } = new();
        public DashboardStatusDto Status { get; set; } = new();
        public List<DashboardTicketDto> RecentTickets { get; set; } = new();
    }
}

