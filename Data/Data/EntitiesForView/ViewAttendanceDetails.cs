using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.EntitiesForView
{
    public class ViewAttendanceDetails
    {
        public int AttendanceId { get; set; }
        public int StudentId { get; set; }
        public int LessonNumber { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public int LessonId { get; set; }
        public DateTime Date { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
        public bool IsPresent { get; set; }
        public bool IsAbsent { get; set; }
        public bool IsLate { get; set; }
        public bool IsExcused { get; set; }
        public bool IsUnexcused { get; set; }
    }
}
