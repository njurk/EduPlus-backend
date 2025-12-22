using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.EntitiesForView
{
    public class ViewBehaviorDetails
    {
        public int NoteId { get; set; }
        public int StudentId { get; set; }
        public int Points { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsPositive { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int SemesterId { get; set; }
    }
}
