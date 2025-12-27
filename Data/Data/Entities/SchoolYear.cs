using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class SchoolYear
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public required string Name { get; set; }

        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
        public virtual ICollection<Semester> Semesters { get; set; } = new List<Semester>();
    }
}
