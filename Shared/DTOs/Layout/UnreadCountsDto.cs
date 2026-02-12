namespace Shared.DTOs
{
    public class UnreadCountsDto
    {
        public int Announcements { get; set; }
        public int Tickets { get; set; }
        public int Excuses { get; set; }
        public bool IsHomeroomTeacher { get; set; }
        public List<int> UnreadAnnouncementIds { get; set; } = [];
    }
}
