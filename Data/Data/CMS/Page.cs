using System.ComponentModel.DataAnnotations;

namespace Data.Data.CMS
{
    public class Page
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Link { get; set; }
        public required int TargetId { get; set; }
        public Target Target { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int? ModifiedByUserId { get; set; }
        public ICollection<PageContent> PageContents { get; set; } = new List<PageContent>();
    }
}
