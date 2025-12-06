using Data.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data
{
    public class DataSeeder
    {
        public static void Seed(SchoolDbContext context)
        {
            if (!context.Database.CanConnect()) return;

            SeedUsers(context);
            SeedTeacherClassSubjects(context);
            SeedClassStudents(context);
            SeedWeeklySchedule(context);
            SeedGrades(context);

        }

        private static void SeedWeeklySchedule(SchoolDbContext context)
        {
            if (context.WeeklySchedules.Any()) return;

            var year = context.SchoolYears.First();
            var semester = context.Semesters.First(s => s.Name == "Semestr 1");
            var class1A = context.Classes.First(c => c.Level == 1 && c.Letter == "A");
            var math = context.Subjects.First(s => s.Name == "Matematyka");
            var teacher = context.Users.First(u => u.Email == "nauczyciel@szkola.pl");
            var room = context.Classrooms.First();
            var hour1 = context.LessonHours.First(l => l.OrderNumber == 1);

            context.WeeklySchedules.Add(new WeeklySchedule
            {
                SchoolYearId = year.Id,
                SemesterId = semester.Id,
                ClassId = class1A.Id,
                SubjectId = math.Id,
                TeacherId = teacher.Id,
                ClassroomId = room.Id,
                DayOfWeek = 1, // zaczynamy od poniedziałku
                LessonHourId = hour1.Id
            });
            context.SaveChanges();
        }
    }
}
