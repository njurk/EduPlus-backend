using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.EntitiesForView
{
    public class ViewUpcomingEvent
    {
        public int EventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDateTime { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string ColorCode { get; set; } = string.Empty;
        public int? ClassId { get; set; }
        public int? ClassSubjectId { get; set; }
        public string? SubjectName { get; set; }
    }
}
