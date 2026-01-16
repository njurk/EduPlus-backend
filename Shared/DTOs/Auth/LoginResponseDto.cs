using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class LoginResponseDto
    {
        public required string Token { get; set; }
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public required List<string> Roles { get; set; }
    }
}
