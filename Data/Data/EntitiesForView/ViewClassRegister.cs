using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Data.Data.EntitiesForView
{
    public class ViewClassRegister
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public int StudentId { get; set; }
        public string StudentFullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}