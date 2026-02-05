namespace Shared.DTOs.Mobile
{
    public class MobileGradesDto
    {
        public List<MobileSubjectGradesDto> Subjects { get; set; } = new();
        public List<MobileRecentGradeDto> RecentGrades { get; set; } = new();
    }
}
