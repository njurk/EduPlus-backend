using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class ClassStudentDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int OrderNumber { get; set; }
        public UserDto Student { get; set; }
    }
}
