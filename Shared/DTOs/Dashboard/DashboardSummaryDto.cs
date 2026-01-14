namespace Shared.DTOs
{
    public class DashboardSummaryDto
    {
        public DashboardStatsDto Stats { get; set; }

        public DashboardStatusDto Status { get; set; }

        public List<DashboardAnnouncementDto> Announcements { get; set; }
    }
}
