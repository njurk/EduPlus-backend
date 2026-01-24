using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class Views : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                create or alter view [dbo].[vw_DashboardStatsView] as
                select
                    (select count(*) from Users where IsActive = 1) as TotalUsers,
                    (select count(*) from Users u join UserRoles ur on u.Id = ur.UserId join Roles r on ur.RoleId = r.Id where r.Level = 4 and u.IsActive = 1) as TotalStudents,
                    (select count(*) from Users u join UserRoles ur on u.Id = ur.UserId join Roles r on ur.RoleId = r.Id where r.Level = 2 and u.IsActive = 1) as TotalTeachers,
                    (select count(*) from Classes where IsActive = 1) as TotalClasses;
            ");

            migrationBuilder.Sql(@"
                create or alter view [dbo].[vw_ParentStudentView] as
                select 
                    ps.Id,
                    ps.ParentId,
                    (p.FirstName + ' ' + p.LastName) as ParentName,
                    p.Email as ParentEmail,
                    ps.StudentId,
                    (s.FirstName + ' ' + s.LastName) as StudentName,
                    ps.CreatedAt,
                    ps.UpdatedAt
                from ParentStudents ps
                join Users p on ps.ParentId = p.Id
                join Users s on ps.StudentId = s.Id;
            ");

            migrationBuilder.Sql(@"
                create or alter view [dbo].[vw_UserListView] as
                select 
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.Phone,
                    u.IsActive,
                    u.CreatedAt,
                    u.UpdatedAt,
                    [dbo].[fn_GetUserRoles](u.Id) as RoleNames,
                    cast(case 
                        when exists(select 1 from UserRoles ur join Roles r on ur.RoleId = r.Id where ur.UserId = u.Id and r.Level = 3) 
                             and not exists(select 1 from ParentStudents ps where ps.ParentId = u.Id) 
                        then 1 else 0 
                    end as bit) as IsUnassignedParent,
                    isnull(m.FirstName + ' ' + m.LastName, 'System') as ModifiedByName
                from Users u
                left join Users m on u.ModifiedByUserId = m.Id;
            ");

            migrationBuilder.Sql(@"
                create or alter view [dbo].[vw_StudentGradesSummary] as
                select
                    u.Id as StudentId,
                    u.FirstName + ' ' + u.LastName as StudentName,
                    c.Id as ClassId,
                    cast(c.Level as varchar) + c.Letter as ClassName,
                    s.Id as SubjectId,
                    s.Name as SubjectName,
                    count(g.Id) as GradeCount,
                    [dbo].[fn_CalculateWeightedAverage](u.Id, s.Id, sy.StartDate, sy.EndDate) as WeightedAverage
                from Users u
                join ClassStudents cs on u.Id = cs.StudentId
                join Classes c on cs.ClassId = c.Id
                join SchoolYears sy on c.SchoolYearId = sy.Id
                join ClassSubjects csub on c.Id = csub.ClassId
                join Subjects s on csub.SubjectId = s.Id
                left join Grades g on u.Id = g.StudentId and s.Id = g.SubjectId and g.IsActive = 1
                where u.IsActive = 1 and c.IsActive = 1
                group by u.Id, u.FirstName, u.LastName, c.Id, c.Level, c.Letter, s.Id, s.Name, sy.StartDate, sy.EndDate;
            ");

            migrationBuilder.Sql(@"
                create or alter view [dbo].[vw_ClassAttendanceSummary] as
                select
                    c.Id as ClassId,
                    cast(c.Level as varchar) + c.Letter as ClassName,
                    count(distinct cs.StudentId) as StudentCount,
                    count(case when at.Slug = 'present' then 1 end) as PresentCount,
                    count(case when at.Slug = 'absent' then 1 end) as AbsentCount,
                    count(case when at.Slug = 'late' then 1 end) as LateCount,
                    count(case when at.Slug = 'excused' then 1 end) as ExcusedCount,
                    cast(round(
                        cast(count(case when at.Slug in ('present', 'late') then 1 end) as float) / 
                        nullif(cast(count(a.Id) as float), 0) * 100, 2
                    ) as decimal(5,2)) as AttendancePercentage
                from Classes c
                join ClassStudents cs on c.Id = cs.ClassId
                join Lessons l on c.Id = l.ClassId
                left join Attendances a on l.Id = a.LessonId and cs.StudentId = a.StudentId
                left join AttendanceTypes at on a.AttendanceTypeId = at.Id
                where c.IsActive = 1
                group by c.Id, c.Level, c.Letter;
            ");

            migrationBuilder.Sql(@"
                create or alter view [dbo].[vw_TeacherSchedule] as
                select
                    ws.Id,
                    ws.DayOfWeek,
                    lh.OrderNumber as LessonNumber,
                    lh.StartTime,
                    lh.EndTime,
                    t.Id as TeacherId,
                    t.FirstName + ' ' + t.LastName as TeacherName,
                    c.Id as ClassId,
                    cast(c.Level as varchar) + c.Letter as ClassName,
                    s.Id as SubjectId,
                    s.Name as SubjectName,
                    cr.Id as ClassroomId,
                    cr.Name as ClassroomName
                from WeeklySchedules ws
                join Users t on ws.TeacherId = t.Id
                join Classes c on ws.ClassId = c.Id
                join Subjects s on ws.SubjectId = s.Id
                join Classrooms cr on ws.ClassroomId = cr.Id
                join LessonHours lh on ws.LessonHourId = lh.Id
                where ws.IsActive = 1;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop view if exists [dbo].[vw_TeacherSchedule]");
            migrationBuilder.Sql("drop view if exists [dbo].[vw_ClassAttendanceSummary]");
            migrationBuilder.Sql("drop view if exists [dbo].[vw_StudentGradesSummary]");
            migrationBuilder.Sql("drop view if exists [dbo].[vw_UserListView]");
            migrationBuilder.Sql("drop view if exists [dbo].[vw_ParentStudentView]");
            migrationBuilder.Sql("drop view if exists [dbo].[vw_DashboardStatsView]");
        }
    }
}
