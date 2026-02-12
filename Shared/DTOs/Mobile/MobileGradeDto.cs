namespace Shared.DTOs.Mobile
{
    public class MobileGradeDto
    {
        public int Id { get; set; }
        public string Value { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public string CategoryColorHex { get; set; } = "";
        public string? CategorySlug { get; set; }
        public string TeacherName { get; set; } = "";
        public string? Comment { get; set; }
        public int Weight { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
