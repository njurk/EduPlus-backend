using System;

namespace Shared.DTOs
{
    public class CreateFromScheduleDto
    {
        public int ScheduleId { get; set; }
        public DateTime Date { get; set; }
        public int? TeacherId { get; set; }
        public int? StatusId { get; set; }
    }
}
