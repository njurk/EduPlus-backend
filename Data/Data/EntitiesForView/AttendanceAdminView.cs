namespace Data.Data.EntitiesForView
{
    public class AttendanceAdminView
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int AttendanceTypeId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string? StudentEmail { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public DateTime LessonDate { get; set; }
        public int OrderNumber { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public string ColorHex { get; set; } = string.Empty;
        public string? ModifiedByName { get; set; }
    }
}
