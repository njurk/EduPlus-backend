using System.ComponentModel.DataAnnotations;
using Data.Data.Entities;

namespace Data.Data.CMS
{
    public class Target
    {
        [Key]
        public int Id { get; set; }
        public required string Label { get; set; }
        public required string Title { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? ModifiedByUserId { get; set; }
        public ICollection<Page> Pages { get; set; } = new List<Page>();
    }
}
