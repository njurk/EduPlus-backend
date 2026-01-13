using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    namespace API.DTOs
    {
        public class AttendanceAdminDto
        {
            public int Id { get; set; }
            public int StudentId { get; set; }
            public string StudentName { get; set; } = string.Empty;
            public string StudentEmail { get; set; } = string.Empty;
            public string SubjectName { get; set; } = string.Empty;
            public string TeacherName { get; set; } = string.Empty;
            public string TypeName { get; set; } = string.Empty;
            public string ShortCode { get; set; } = string.Empty;
            public DateTime LessonDate { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public bool IsActive { get; set; }
        }
    }
}
