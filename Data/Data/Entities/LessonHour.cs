using System.ComponentModel.DataAnnotations;

namespace Data.Data.Entities
{
    public class LessonHour
    {
        [Key]
        public int Id { get; set; }
        public int OrderNumber { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
