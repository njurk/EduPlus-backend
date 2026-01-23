using Data.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Data.Data.Entities
{
    public class LessonStatus : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? ModifiedByUserId { get; set; }
    }
}

