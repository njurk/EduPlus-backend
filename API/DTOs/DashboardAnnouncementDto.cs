namespace API.DTOs
{
    public class DashboardAnnouncementDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Date { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;
    }
}
