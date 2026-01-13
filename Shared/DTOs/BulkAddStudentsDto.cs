using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class BulkAddStudentsDto
    {
        public int ClassId { get; set; }
        public List<int> StudentIds { get; set; } = new();
    }
}
