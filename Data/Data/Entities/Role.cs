using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required int Level { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
