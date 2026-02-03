using Data.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Data.Data.Entities
{
    public class AttendanceType : IAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Slug { get; set; }
        public required string ShortCode { get; set; }
        public string ColorHex { get; set; } = "#6b7280";
        public bool IsNegative { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? ModifiedByUserId { get; set; }
    }
}

