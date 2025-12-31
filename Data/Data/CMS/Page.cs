using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
