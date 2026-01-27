namespace Data.Data.EntitiesForView
{
    public class LessonsAdminView
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int TeacherId { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public int? ClassroomId { get; set; }
        public string? ClassroomName { get; set; }
        public int LessonHourId { get; set; }
        public int OrderNumber { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateTime Date { get; set; }
        public string Topic { get; set; } = string.Empty;
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public string? ModifiedByName { get; set; }
    }
}
