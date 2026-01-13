using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public class GradeDto
    {
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int GradeTypeId { get; set; }
        public int GradeCategoryId { get; set; }
        public string? Comment { get; set; }
    }
}
