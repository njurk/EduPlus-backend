using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class BehaviorGradeRange
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string GradeName { get; set; }

        [Required]
        public int MinPoints { get; set; }

        [Required]
        public int MaxPoints { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
