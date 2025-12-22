using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class BehaviorGradeRange
    {
        [Key]
        public int Id { get; set; }
        public int SchoolYearId { get; set; }
        [ForeignKey(nameof(SchoolYearId))]
        public virtual SchoolYear SchoolYear { get; set; } = null!;
        public required string GradeName { get; set; }
        public int MinPoints { get; set; }
        public int MaxPoints { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
