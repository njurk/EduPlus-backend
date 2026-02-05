namespace Shared.DTOs.Mobile
{
    public class MobileSubjectGradesDto
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = "";
        public double? Average { get; set; }
        public List<MobileGradeDto> Grades { get; set; } = new();
    }
}
