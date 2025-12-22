using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.EntitiesForView
{
    public class ViewBehaviorGradeSummary
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public int SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public string SchoolYearName { get; set; } = string.Empty;
        public int TotalPoints { get; set; }
        public string? CalculatedGradeName { get; set; }
    }
}
