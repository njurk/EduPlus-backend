using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Data.Entities;

namespace Data.Data.CMS
{
    public class PageContent
    {
        [Key]
        public int Id { get; set; }
        public required string Key { get; set; }
        public required string Value { get; set; }
        public required int PageId { get; set; }
        public Page Page { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? ModifiedByUserId { get; set; }
        [ForeignKey(nameof(ModifiedByUserId))]
        public virtual User? ModifiedByUser { get; set; }
    }
}

