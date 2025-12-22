using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.EntitiesForView
{
    public class ViewLessonSchedule
    {
        public int ScheduleId { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int DayOfWeek { get; set; }
        public int LessonNumber { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string ClassroomName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public int SchoolYearId { get; set; }
        public int SemesterId { get; set; }
    }
}
