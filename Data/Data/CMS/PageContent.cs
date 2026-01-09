using System.ComponentModel.DataAnnotations;

namespace Data.Data.CMS
{
    public class PageContent
    {
        [Key]
        public int Id { get; set; }
        public required string Label { get; set; }
        public required string Content { get; set; }
        public required int PageId { get; set; }
        public Page Page { get; set; }
    }
}
