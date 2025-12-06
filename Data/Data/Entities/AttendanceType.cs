using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class AttendanceType
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public required string Name { get; set; }

        [Required, MaxLength(5)]
        public required string ShortCode { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
