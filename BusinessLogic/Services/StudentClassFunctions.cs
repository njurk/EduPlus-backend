using Data.Data;
using Data.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class StudentClassFunctions
    {
        private readonly SchoolDbContext _context;
        public StudentClassFunctions(SchoolDbContext context)
        {
            _context = context;
        }

        public async Task AddStudentToClass(int classId, int studentId)
        {
            var maxOrder = await _context.ClassStudents
                .Where(cs => cs.ClassId == classId)
                .MaxAsync(cs => (int?)cs.OrderNumber) ?? 0;

            var classStudent = new ClassStudent
            {
                ClassId = classId,
                StudentId = studentId,
                OrderNumber = maxOrder + 1,
                CreatedAt = DateTime.Now
            };

            _context.ClassStudents.Add(classStudent);
            await _context.SaveChangesAsync();
        }
    }
}
