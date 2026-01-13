using System.ComponentModel.DataAnnotations;

namespace Data.Data.Entities
{
    public class SchoolYear
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
        public virtual ICollection<Semester> Semesters { get; set; } = new List<Semester>();
    }
}
