using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.Entities
{
    public class SharedFiles
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public required string Name { get; set; }

        [Required, MaxLength(255)]
        public required string Path { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public int UploadedBy { get; set; }
        [ForeignKey(nameof(UploadedBy))]
        public virtual User Uploader { get; set; } = null!;

        public bool IsActive { get; set; } = true;
    }
}
