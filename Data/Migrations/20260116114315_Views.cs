using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class Views : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_DashboardStatsView] AS
                SELECT
                    (SELECT COUNT(*) FROM Users WHERE IsActive = 1) AS TotalUsers,
                    (SELECT COUNT(*) FROM Users u JOIN UserRoles ur ON u.Id = ur.UserId JOIN Roles r ON ur.RoleId = r.Id WHERE r.Name = 'Uczen' AND u.IsActive = 1) AS TotalStudents,
                    (SELECT COUNT(*) FROM Users u JOIN UserRoles ur ON u.Id = ur.UserId JOIN Roles r ON ur.RoleId = r.Id WHERE r.Name = 'Nauczyciel' AND u.IsActive = 1) AS TotalTeachers,
                    (SELECT COUNT(*) FROM Classes WHERE IsActive = 1) AS TotalClasses;
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_ParentStudentView] AS
                SELECT 
                    ps.Id,
                    ps.ParentId,
                    (p.FirstName + ' ' + p.LastName) AS ParentName,
                    p.Email AS ParentEmail,
                    ps.StudentId,
                    (s.FirstName + ' ' + s.LastName) AS StudentName,
                    ps.CreatedAt,
                    ps.UpdatedAt
                FROM ParentStudents ps
                JOIN Users p ON ps.ParentId = p.Id
                JOIN Users s ON ps.StudentId = s.Id;
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_UserListView] AS
                SELECT 
                    u.Id,
                    u.FirstName,
                    u.LastName,
                    u.Email,
                    u.Phone,
                    u.IsActive,
                    u.CreatedAt,
                    u.UpdatedAt,
                    [dbo].[fn_GetUserRoles](u.Id) AS RoleNames,
                    CAST(CASE 
                        WHEN EXISTS(SELECT 1 FROM UserRoles ur JOIN Roles r ON ur.RoleId = r.Id WHERE ur.UserId = u.Id AND r.Name = 'Rodzic') 
                             AND NOT EXISTS(SELECT 1 FROM ParentStudents ps WHERE ps.ParentId = u.Id) 
                        THEN 1 ELSE 0 
                    END AS BIT) AS IsUnassignedParent,
                    ISNULL(m.FirstName + ' ' + m.LastName, 'System') AS ModifiedByName
                FROM Users u
                LEFT JOIN Users m ON u.ModifiedByUserId = m.Id;
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_StudentGradesSummary] AS
                SELECT
                    u.Id AS StudentId,
                    u.FirstName + ' ' + u.LastName AS StudentName,
                    c.Id AS ClassId,
                    CAST(c.Level AS VARCHAR) + c.Letter AS ClassName,
                    s.Id AS SubjectId,
                    s.Name AS SubjectName,
                    COUNT(g.Id) AS GradeCount,
                    [dbo].[fn_CalculateWeightedAverage](u.Id, s.Id, sy.StartDate, sy.EndDate) AS WeightedAverage
                FROM Users u
                JOIN ClassStudents cs ON u.Id = cs.StudentId
                JOIN Classes c ON cs.ClassId = c.Id
                JOIN SchoolYears sy ON c.SchoolYearId = sy.Id
                JOIN ClassSubjects csub ON c.Id = csub.ClassId
                JOIN Subjects s ON csub.SubjectId = s.Id
                LEFT JOIN Grades g ON u.Id = g.StudentId AND s.Id = g.SubjectId AND g.IsActive = 1
                WHERE u.IsActive = 1 AND c.IsActive = 1
                GROUP BY u.Id, u.FirstName, u.LastName, c.Id, c.Level, c.Letter, s.Id, s.Name, sy.StartDate, sy.EndDate;
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_ClassAttendanceSummary] AS
                SELECT
                    c.Id AS ClassId,
                    CAST(c.Level AS VARCHAR) + c.Letter AS ClassName,
                    COUNT(DISTINCT cs.StudentId) AS StudentCount,
                    COUNT(CASE WHEN at.ShortCode = 'OB' THEN 1 END) AS PresentCount,
                    COUNT(CASE WHEN at.ShortCode = 'NB' THEN 1 END) AS AbsentCount,
                    COUNT(CASE WHEN at.ShortCode = 'SP' THEN 1 END) AS LateCount,
                    COUNT(CASE WHEN at.ShortCode = 'U' THEN 1 END) AS ExcusedCount,
                    CAST(ROUND(
                        CAST(COUNT(CASE WHEN at.ShortCode IN ('OB', 'SP') THEN 1 END) AS FLOAT) / 
                        NULLIF(CAST(COUNT(a.Id) AS FLOAT), 0) * 100, 2
                    ) AS DECIMAL(5,2)) AS AttendancePercentage
                FROM Classes c
                JOIN ClassStudents cs ON c.Id = cs.ClassId
                JOIN Lessons l ON c.Id = l.ClassId
                LEFT JOIN Attendances a ON l.Id = a.LessonId AND cs.StudentId = a.StudentId
                LEFT JOIN AttendanceTypes at ON a.AttendanceTypeId = at.Id
                WHERE c.IsActive = 1
                GROUP BY c.Id, c.Level, c.Letter;
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_TeacherSchedule] AS
                SELECT
                    ws.Id,
                    ws.DayOfWeek,
                    lh.OrderNumber AS LessonNumber,
                    lh.StartTime,
                    lh.EndTime,
                    t.Id AS TeacherId,
                    t.FirstName + ' ' + t.LastName AS TeacherName,
                    c.Id AS ClassId,
                    CAST(c.Level AS VARCHAR) + c.Letter AS ClassName,
                    s.Id AS SubjectId,
                    s.Name AS SubjectName,
                    cr.Id AS ClassroomId,
                    cr.Name AS ClassroomName
                FROM WeeklySchedules ws
                JOIN Users t ON ws.TeacherId = t.Id
                JOIN Classes c ON ws.ClassId = c.Id
                JOIN Subjects s ON ws.SubjectId = s.Id
                JOIN Classrooms cr ON ws.ClassroomId = cr.Id
                JOIN LessonHours lh ON ws.LessonHourId = lh.Id
                WHERE ws.IsActive = 1;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_TeacherSchedule]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_ClassAttendanceSummary]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_StudentGradesSummary]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_UserListView]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_ParentStudentView]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_DashboardStatsView]");
        }
    }
}
