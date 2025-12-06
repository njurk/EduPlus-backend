using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class GradeCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required, MaxLength(20)]
        public required string Name { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
