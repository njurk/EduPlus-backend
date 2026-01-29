using Data.Data.CMS;
using Data.Data.Entities;
using Data.Data.EntitiesForView;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class EduPlusDbContext : DbContext
    {
        public EduPlusDbContext(DbContextOptions<EduPlusDbContext> options) : base(options)
        {
        }

        public DbSet<Announcement> Announcements { get; set; } = null!;
        public DbSet<AnnouncementRead> AnnouncementReads { get; set; } = null!;
        public DbSet<AnnouncementTarget> AnnouncementTargets { get; set; } = null!;
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
        public DbSet<SubjectTeacher> SubjectTeachers { get; set; }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserRole> UserRoles { get; set; } = null!;
        public DbSet<WeeklySchedule> WeeklySchedules { get; set; } = null!;
        public DbSet<ParentStudent> ParentStudents { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketReason> TicketReasons { get; set; }
        public DbSet<TicketRead> TicketReads { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<Page> Pages { get; set; } = null!;
        public DbSet<Target> Targets { get; set; } = null!;
        public DbSet<PageContent> PageContents { get; set; } = null!;

        public DbSet<DashboardStatsView> DashboardStats { get; set; }
        public DbSet<UserListView> UserList { get; set; }
        public DbSet<ParentStudentView> ParentStudentList { get; set; }
        public DbSet<AttendanceAdminView> AttendanceAdminList { get; set; }
        public DbSet<GradesAdminView> GradesAdminList { get; set; }
        public DbSet<LessonsAdminView> LessonsAdminList { get; set; }


        [DbFunction("fn_CalculateWeightedAverage", "dbo")]
        public static decimal CalculateWeightedAverage(int studentId, int subjectId, DateTime startDate, DateTime endDate)
        {
            throw new NotSupportedException();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var initialDateTime = new DateTime(2025, 12, 27, 22, 0, 0, DateTimeKind.Utc);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
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
                e.HasIndex(u => new { u.LastName, u.FirstName });
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
            });

            modelBuilder.Entity<Class>()
                .HasIndex(c => new { c.SchoolYearId, c.Level, c.Letter })
                .IsUnique();

            modelBuilder.Entity<ClassStudent>(e =>
            {
                e.HasIndex(cs => new { cs.ClassId, cs.StudentId })
                 .IsUnique();
            });

            modelBuilder.Entity<TeacherClassSubject>(e =>
            {
                e.HasIndex(tcs => new { tcs.TeacherId, tcs.ClassId, tcs.SubjectId })
                 .IsUnique();
                e.HasOne(x => x.Teacher).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SubjectTeacher>()
                .HasKey(st => new { st.SubjectId, st.TeacherId });

            modelBuilder.Entity<SubjectTeacher>()
                .HasOne(st => st.Subject)
                .WithMany()
                .HasForeignKey(st => st.SubjectId);

            modelBuilder.Entity<SubjectTeacher>()
                .HasOne(st => st.Teacher)
                .WithMany()
                .HasForeignKey(st => st.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Attendance>(e =>
            {
                e.HasIndex(a => new { a.LessonId, a.StudentId })
                 .IsUnique();
                e.HasIndex(a => new { a.IsActive, a.LessonId });
                e.HasOne(x => x.Student).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Grade>(e =>
            {
                e.HasIndex(g => new { g.StudentId, g.SubjectId, g.DateTime });
                e.HasIndex(g => new { g.IsActive, g.CreatedAt });
                e.HasOne(g => g.Student).WithMany().OnDelete(DeleteBehavior.Restrict);
                e.HasOne(g => g.Teacher).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Announcement>(e =>
            {
                e.HasIndex(a => new { a.CreatedAt, a.AuthorId });
                e.HasOne(a => a.Author).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<AnnouncementRead>(e =>
            {
                e.HasIndex(ar => new { ar.AnnouncementId, ar.UserId }).IsUnique();
                e.HasOne(ar => ar.User).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Lesson>(e =>
            {
                e.HasIndex(l => new { l.Date, l.ClassId, l.SubjectId });
                e.HasIndex(l => l.LessonHourId);
                e.HasIndex(l => l.TeacherId);
                e.HasIndex(l => l.SubjectId);
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

            modelBuilder.Entity<Ticket>(e =>
            {
                e.HasIndex(t => t.Email);
                e.HasOne(t => t.Reason)
                    .WithMany()
                    .HasForeignKey(t => t.ReasonId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<GradeType>().Property(p => p.Value).HasColumnType("decimal(2,1)");

            modelBuilder.HasDbFunction(typeof(EduPlusDbContext).GetMethod(nameof(CalculateWeightedAverage))!)
                .HasName("fn_CalculateWeightedAverage")
                .HasSchema("dbo");

            modelBuilder.Entity<DashboardStatsView>().HasNoKey().ToView("vw_DashboardStatsView");
            modelBuilder.Entity<UserListView>().HasNoKey().ToView("vw_UserListView");
            modelBuilder.Entity<ParentStudentView>().HasNoKey().ToView("vw_ParentStudentView");
            modelBuilder.Entity<AttendanceAdminView>().HasNoKey().ToView("vw_AttendanceAdmin");
            modelBuilder.Entity<GradesAdminView>().HasNoKey().ToView("vw_GradesAdmin");
            modelBuilder.Entity<LessonsAdminView>().HasNoKey().ToView("vw_LessonsAdmin");

            ModelSeeder.Seed(modelBuilder);
        }

        public void ApplySqlObjects()
        {
            Database.ExecuteSqlRaw(Sql.SqlFunctions.CalculateWeightedAverage);
            Database.ExecuteSqlRaw(Sql.SqlFunctions.GetUserRoles);

            Database.ExecuteSqlRaw(Sql.SqlProcedures.GenerateNextSchoolYear);
            Database.ExecuteSqlRaw(Sql.SqlProcedures.DeactivateLessonAttendance);
            Database.ExecuteSqlRaw(Sql.SqlProcedures.GenerateLessonAttendance);
            Database.ExecuteSqlRaw(Sql.SqlProcedures.RecalculateClassStudentOrder);

            Database.ExecuteSqlRaw(Sql.SqlViews.DashboardStats);
            Database.ExecuteSqlRaw(Sql.SqlViews.ParentStudent);
            Database.ExecuteSqlRaw(Sql.SqlViews.UserList);
            Database.ExecuteSqlRaw(Sql.SqlViews.StudentGradesSummary);
            Database.ExecuteSqlRaw(Sql.SqlViews.ClassAttendanceSummary);
            Database.ExecuteSqlRaw(Sql.SqlViews.TeacherSchedule);
            Database.ExecuteSqlRaw(Sql.SqlViews.AttendanceAdmin);
            Database.ExecuteSqlRaw(Sql.SqlViews.GradesAdmin);
            Database.ExecuteSqlRaw(Sql.SqlViews.LessonsAdmin);
        }
    }
}
