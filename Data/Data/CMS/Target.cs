using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data.CMS
{
    public class Target
    {
        [Key]
        public int Id { get; set; }
        public required string Label { get; set; }
        public required string Title { get; set; }
        public ICollection<Page> Pages { get; set; }
    }
}
