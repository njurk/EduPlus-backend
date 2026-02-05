using System;

namespace Shared.DTOs
{
    public class UpdateLessonDto
    {
        public string? Topic { get; set; }
        public int? StatusId { get; set; }
        public int? ClassroomId { get; set; }
        public int? TeacherId { get; set; }
    }
}
