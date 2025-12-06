using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class BehaviorNoteType
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public required string Name { get; set; }

        [Required]
        public bool IsPositive { get; set; }

        [Required]
        public int DefaultPoints { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
