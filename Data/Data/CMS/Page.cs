using System.ComponentModel.DataAnnotations;

namespace Data.Data.CMS
{
    public class Page
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Link { get; set; }
        public required int Position { get; set; }
        public required int TargetId { get; set; }
        public Target Target { get; set; }
        public ICollection<PageContent> PageContents { get; set; }
    }
}
