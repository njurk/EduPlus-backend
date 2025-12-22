using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.EntitiesForView
{
    public class ViewTeacherClass
    {
        public int TeacherId { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int SchoolYearId { get; set; }
        public string SchoolYearName { get; set; } = string.Empty; 
        public string MainSubjectName { get; set; } = string.Empty;
    }
}
