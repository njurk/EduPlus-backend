namespace Shared.DTOs.Mobile
{
    public class MobileGradesDto
    {
        public List<MobileSubjectGradesDto> Subjects { get; set; } = new();
        public List<MobileRecentGradeDto> RecentGrades { get; set; } = new();
    }

    public class MobileSubjectGradesDto
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = "";
        public double? Average { get; set; }
        public List<MobileGradeDto> Grades { get; set; } = new();
    }

    public class MobileGradeDto
    {
        public int Id { get; set; }
        public string Value { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public string CategoryColorHex { get; set; } = "";
        public string TeacherName { get; set; } = "";
        public string? Comment { get; set; }
        public int Weight { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class MobileRecentGradeDto
    {
        public int Id { get; set; }
        public string SubjectName { get; set; } = "";
        public string Value { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public string CategoryColorHex { get; set; } = "";
        public string TeacherName { get; set; } = "";
        public string? Comment { get; set; }
        public int Weight { get; set; }
        public string Date { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}
