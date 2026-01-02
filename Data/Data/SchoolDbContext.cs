using Data.Data.CMS;
using Data.Data.Entities;
using Data.Data.EntitiesForView;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Linq.Expressions;

namespace Data.Data
{
    public class SchoolDbContext : DbContext
    {
        public SchoolDbContext(DbContextOptions<SchoolDbContext> options) : base(options)
        {
        }

        public DbSet<Announcement> Announcements { get; set; } = null!;
        public DbSet<AnnouncementRead> AnnouncementReads { get; set; } = null!;
        public DbSet<Attendance> Attendances { get; set; } = null!;
        public DbSet<AttendanceType> AttendanceTypes { get; set; } = null!;
        public DbSet<Class> Classes { get; set; } = null!;
        public DbSet<Classroom> Classrooms { get; set; } = null!;
        public DbSet<ClassStudent> ClassStudents { get; set; } = null!;
        public DbSet<ClassSubject> ClassSubjects { get; set; } = null!;
        public DbSet<Excuse> Excuses { get; set; } = null!;
        public DbSet<Grade> Grades { get; set; } = null!;
        public DbSet<GradeCategory> GradeCategories { get; set; } = null!;
        public DbSet<GradeType> GradeTypes { get; set; } = null!;
        public DbSet<Lesson> Lessons { get; set; } = null!;
        public DbSet<LessonHour> LessonHours { get; set; } = null!;
        public DbSet<LessonStatus> LessonStatuses { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<SchoolYear> SchoolYears { get; set; } = null!;
        public DbSet<Semester> Semesters { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<TeacherClassSubject> TeacherClassSubjects { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<WeeklySchedule> WeeklySchedules { get; set; } = null!;
        public DbSet<ParentStudent> ParentStudents { get; set; } = null!;

        // widoki
        public DbSet<ViewStudentAttendance> ViewStudentAttendances { get; set; } = null!;
        public DbSet<ViewLessonSchedule> ViewLessonSchedules { get; set; } = null!;
        public DbSet<ViewGradeDetails> ViewGradeDetails { get; set; } = null!;
        public DbSet<ViewAttendanceDetails> ViewAttendanceDetails { get; set; } = null!;
        public DbSet<ViewAnnouncementDetails> ViewAnnouncementDetails { get; set; } = null!;
        public DbSet<ViewTeacherClass> ViewTeacherClasses { get; set; } = null!;
        public DbSet<ViewClassStudentDetails> ViewClassStudentDetails { get; set; } = null!;
        public DbSet<ViewPendingExcuse> ViewPendingExcuses { get; set; } = null!;
        public DbSet<Page> Pages { get; set; } = null!;
        public DbSet<PageContent> PageContents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var initialDateTime = new DateTime(2025, 12, 27, 22, 0, 0, DateTimeKind.Utc);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // automatyczne timestampy
                var createdAt = entityType.FindProperty("CreatedAt");
                if (createdAt != null)
                {
                    createdAt.SetDefaultValueSql("GETUTCDATE()");
                }

                var updatedAt = entityType.FindProperty("UpdatedAt");
                if (updatedAt != null)
                {
                    updatedAt.SetDefaultValueSql("GETUTCDATE()");
                }
            }

            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
                e.Property(u => u.LastName).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<UserRole>(e =>
            {
                e.HasIndex(ur => new { ur.UserId, ur.RoleId }).IsUnique();
            });

            modelBuilder.Entity<ParentStudent>(e =>
            {
                e.HasIndex(ps => new { ps.ParentId, ps.StudentId }).IsUnique();
                e.HasOne(ps => ps.Parent).WithMany().OnDelete(DeleteBehavior.Restrict);
                e.HasOne(ps => ps.Student).WithMany().OnDelete(DeleteBehavior.Restrict);
                e.HasQueryFilter(x => x.IsActive);
            });

            modelBuilder.Entity<ClassStudent>(e =>
            {
                e.HasIndex(cs => new { cs.ClassId, cs.StudentId })
                 .IsUnique()
                 .HasFilter("[IsActive] = 1");
            });

            modelBuilder.Entity<TeacherClassSubject>(e =>
            {
                e.HasIndex(tcs => new { tcs.TeacherId, tcs.ClassId, tcs.SubjectId })
                 .IsUnique()
                 .HasFilter("[IsActive] = 1");

                e.HasOne(x => x.Teacher).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Attendance>(e =>
            {
                e.HasIndex(a => new { a.LessonId, a.StudentId })
                 .IsUnique()
                 .HasFilter("[IsActive] = 1");

                e.HasOne(x => x.Student).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Grade>(e =>
            {
                e.HasOne(g => g.Student).WithMany().OnDelete(DeleteBehavior.Restrict);
                e.HasOne(g => g.Teacher).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Announcement>(e =>
            {
                e.HasOne(a => a.Author).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AnnouncementRead>(e =>
            {
                e.HasIndex(ar => new { ar.AnnouncementId, ar.UserId }).IsUnique();
                e.HasOne(ar => ar.User).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Lesson>(e =>
            {
                e.HasOne(l => l.Teacher).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<WeeklySchedule>(e =>
            {
                e.HasOne(ws => ws.SchoolYear)
                 .WithMany()
                 .HasForeignKey(ws => ws.SchoolYearId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ws => ws.Semester)
                 .WithMany()
                 .HasForeignKey(ws => ws.SemesterId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ws => ws.Class)
                 .WithMany()
                 .HasForeignKey(ws => ws.ClassId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(ws => ws.Subject)
                 .WithMany()
                 .HasForeignKey(ws => ws.SubjectId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<GradeType>().Property(p => p.Value).HasColumnType("decimal(2,1)");

            // widoki
            modelBuilder.Entity<ViewStudentAttendance>().HasNoKey().ToView("vw_StudentAttendanceSummary");
            modelBuilder.Entity<ViewGradeDetails>().HasNoKey().ToView("vw_GradeDetails");
            modelBuilder.Entity<ViewAnnouncementDetails>().HasNoKey().ToView("vw_AnnouncementDetails");
            modelBuilder.Entity<ViewLessonSchedule>().HasNoKey().ToView("vw_LessonSchedule");
            modelBuilder.Entity<ViewAttendanceDetails>().HasNoKey().ToView("vw_AttendanceDetails");
            modelBuilder.Entity<ViewTeacherClass>().HasNoKey().ToView("vw_TeacherClasses");
            modelBuilder.Entity<ViewClassStudentDetails>().HasNoKey().ToView("vw_ClassStudentDetails");
            modelBuilder.Entity<ViewPendingExcuse>().HasNoKey().ToView("vw_PendingExcuses");

            // seedowanie tabel (reszta w DataSeeder)
            modelBuilder.Entity<Target>().HasData(
                new Target { Id = 1, Label = "WebAdmin", Title = "Administrator - strona internetowa" },
                new Target { Id = 2, Label = "MobileTeacher", Title = "Nauczyciel - aplikacja mobilna" }
            );

            modelBuilder.Entity<Page>().HasData(
                new Page { Id = 1, TargetId = 1, Position = 1, Title = "Pulpit", Link = "/admin/dashboard" },
                new Page { Id = 2, TargetId = 1, Position = 2, Title = "Ogłoszenia", Link = "/admin/announcements" },
                new Page { Id = 3, TargetId = 1, Position = 3, Title = "Frekwencja", Link = "/admin/attendance" },
                new Page { Id = 4, TargetId = 1, Position = 4, Title = "Oceny", Link = "/admin/grades" },
                new Page { Id = 5, TargetId = 1, Position = 5, Title = "Klasy", Link = "/admin/classes" },
                new Page { Id = 6, TargetId = 1, Position = 6, Title = "Rok szkolny", Link = "/admin/school-years" },
                new Page { Id = 7, TargetId = 1, Position = 7, Title = "Użytkownicy", Link = "/admin/users" },
                new Page { Id = 8, TargetId = 1, Position = 8, Title = "Role", Link = "/admin/roles" },
                new Page { Id = 9, TargetId = 1, Position = 9, Title = "Przedmioty", Link = "/admin/subjects" },
                new Page { Id = 10, TargetId = 1, Position = 10, Title = "Plany zajęć", Link = "/admin/schedules" },
                new Page { Id = 11, TargetId = 1, Position = 11, Title = "CMS", Link = "/admin/cms" },
                new Page { Id = 12, TargetId = 2, Position = 1, Title = "Pulpit", Link = "/teacher/dashboard" },
                new Page { Id = 13, TargetId = 2, Position = 2, Title = "Klasy", Link = "/teacher/classes" },
                new Page { Id = 14, TargetId = 2, Position = 3, Title = "Plan zajęć", Link = "/teacher/schedule" },
                new Page { Id = 15, TargetId = 2, Position = 4, Title = "Ogłoszenia", Link = "/teacher/announcements" }
            );

            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Administrator", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Role { Id = 2, Name = "Nauczyciel", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Role { Id = 3, Name = "Rodzic", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Role { Id = 4, Name = "Uczeń", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime }
            );

            modelBuilder.Entity<SchoolYear>().HasData(
                new SchoolYear { Id = 1, Name = "2025/2026", StartDate = new DateOnly(2025, 9, 1), EndDate = new DateOnly(2026, 6, 30), IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new SchoolYear { Id = 2, Name = "2026/2027", StartDate = new DateOnly(2026, 9, 1), EndDate = new DateOnly(2027, 6, 30), IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime }
            );

            modelBuilder.Entity<AttendanceType>().HasData(
                new AttendanceType { Id = 1, Name = "Obecność", ShortCode = "OB", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new AttendanceType { Id = 2, Name = "Nieobecność", ShortCode = "NB", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new AttendanceType { Id = 3, Name = "Spóźnienie", ShortCode = "SP", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new AttendanceType { Id = 4, Name = "Usprawiedliwione", ShortCode = "U", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new AttendanceType { Id = 5, Name = "Zwolnienie", ShortCode = "ZW", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime }
            );

            modelBuilder.Entity<GradeType>().HasData(
                new GradeType { Id = 1, Numeric = "1", Name = "Niedostateczny", Value = 1.0m, IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new GradeType { Id = 2, Numeric = "2", Name = "Dopuszczający", Value = 2.0m, IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new GradeType { Id = 3, Numeric = "3", Name = "Dostateczny", Value = 3.0m, IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new GradeType { Id = 4, Numeric = "4", Name = "Dobry", Value = 4.0m, IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new GradeType { Id = 5, Numeric = "5", Name = "Bardzo dobry", Value = 5.0m, IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new GradeType { Id = 6, Numeric = "6", Name = "Celujący", Value = 6.0m, IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime }
            );

            modelBuilder.Entity<GradeCategory>().HasData(
                new GradeCategory { Id = 1, Name = "Sprawdzian", Weight = 3, IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new GradeCategory { Id = 2, Name = "Kartkówka", Weight = 2, IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new GradeCategory { Id = 3, Name = "Odpowiedź ustna", Weight = 1, IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new GradeCategory { Id = 4, Name = "Aktywność", Weight = 1, IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new GradeCategory { Id = 5, Name = "Zadanie domowe", Weight = 1, IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime }
            );

            modelBuilder.Entity<LessonStatus>().HasData(
                new LessonStatus { Id = 1, Name = "Zaplanowana", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new LessonStatus { Id = 2, Name = "Zrealizowana", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new LessonStatus { Id = 3, Name = "Odwołana", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime }
            );

            modelBuilder.Entity<LessonHour>().HasData(
                new LessonHour { Id = 1, OrderNumber = 1, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(8, 45), IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new LessonHour { Id = 2, OrderNumber = 2, StartTime = new TimeOnly(8, 55), EndTime = new TimeOnly(9, 40), IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new LessonHour { Id = 3, OrderNumber = 3, StartTime = new TimeOnly(9, 50), EndTime = new TimeOnly(10, 35), IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new LessonHour { Id = 4, OrderNumber = 4, StartTime = new TimeOnly(10, 45), EndTime = new TimeOnly(11, 30), IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new LessonHour { Id = 5, OrderNumber = 5, StartTime = new TimeOnly(11, 45), EndTime = new TimeOnly(12, 30), IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new LessonHour { Id = 6, OrderNumber = 6, StartTime = new TimeOnly(12, 50), EndTime = new TimeOnly(13, 35), IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new LessonHour { Id = 7, OrderNumber = 7, StartTime = new TimeOnly(13, 45), EndTime = new TimeOnly(14, 30), IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new LessonHour { Id = 8, OrderNumber = 8, StartTime = new TimeOnly(14, 40), EndTime = new TimeOnly(15, 25), IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new LessonHour { Id = 9, OrderNumber = 9, StartTime = new TimeOnly(15, 30), EndTime = new TimeOnly(16, 15), IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime }
            );

            modelBuilder.Entity<Classroom>().HasData(
                new Classroom { Id = 1, Name = "101", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 2, Name = "102", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 3, Name = "103", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 4, Name = "104", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 5, Name = "105", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 6, Name = "201", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 7, Name = "202", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 8, Name = "203", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 9, Name = "204", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 10, Name = "205", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 11, Name = "301", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 12, Name = "302", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 13, Name = "303", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 14, Name = "304", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 15, Name = "305", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 16, Name = "gimnastyczna 1", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 17, Name = "gimnastyczna 2", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Classroom { Id = 18, Name = "aula", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime }
            );

            modelBuilder.Entity<Subject>().HasData(
                new Subject { Id = 1, Name = "matematyka", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 2, Name = "język polski", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 3, Name = "język angielski", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 4, Name = "język niemiecki", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 5, Name = "informatyka", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 6, Name = "wychowanie fizyczne", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 7, Name = "historia", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 8, Name = "WOS", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 9, Name = "biologia", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 10, Name = "chemia", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 11, Name = "fizyka", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 12, Name = "Geografia", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 13, Name = "przyroda", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 14, Name = "plastyka", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 15, Name = "muzyka", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 16, Name = "zajęcia artystyczne", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 17, Name = "religia", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 18, Name = "etyka", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 19, Name = "WDŻ", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 20, Name = "technika", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Subject { Id = 21, Name = "EDB", IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime }
            );

            modelBuilder.Entity<Semester>().HasData(
                new Semester { Id = 1, SchoolYearId = 1, Name = "Semestr 1", StartDate = new DateOnly(2025, 09, 01), EndDate = new DateOnly(2025, 01, 31), IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Semester { Id = 2, SchoolYearId = 1, Name = "Semestr 2", StartDate = new DateOnly(2026, 02, 01), EndDate = new DateOnly(2025, 06, 30), IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Semester { Id = 3, SchoolYearId = 2, Name = "Semestr 1", StartDate = new DateOnly(2026, 09, 01), EndDate = new DateOnly(2026, 01, 31), IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Semester { Id = 4, SchoolYearId = 2, Name = "Semestr 2", StartDate = new DateOnly(2027, 02, 01), EndDate = new DateOnly(2026, 06, 30), IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime }
            );

            modelBuilder.Entity<Class>().HasData(
                new Class { Id = 1, Level = 1, Letter = "A", SchoolYearId = 1, IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime },
                new Class { Id = 2, Level = 8, Letter = "C", SchoolYearId = 1, IsActive = true, CreatedAt = initialDateTime, UpdatedAt = initialDateTime }
            );
        }
    }
}