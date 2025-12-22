using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.EntitiesForView
{
    public class ViewPendingExcuse
    {
        public int ExcuseId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public DateTime LessonDate { get; set; }
        public int LessonNumber { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string ParentName { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public int TeacherId { get; set; } // do filtrowania dla wychowawcy/nauczyciela przedmiotu
    }
}
