using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Data.EntitiesForView
{
    public class ViewGradeDetails
    {
        public int GradeId { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        [Column(TypeName = "decimal(4,2)")]
        public decimal Value { get; set; }
        public string GradeTypeName { get; set; } = string.Empty;
        public int Weight { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? Comment { get; set; }
        public int SemesterId { get; set; }
    }
}