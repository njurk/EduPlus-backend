namespace Shared.DTOs.Mobile
{
    public class MobileScheduleDto
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = "";
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = "";
        public List<MobileLessonDto> Lessons { get; set; } = new();
    }
}
