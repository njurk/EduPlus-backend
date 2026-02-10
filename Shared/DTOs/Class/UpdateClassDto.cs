using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class UpdateClassDto
    {
        public int Level { get; set; }
        public string Letter { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int? HomeroomTeacherId { get; set; }
    }
}
