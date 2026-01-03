using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Views : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_DashboardStats] AS
                SELECT
                    (SELECT COUNT(*) FROM Users WHERE IsActive = 1) AS TotalUsers,
                    (SELECT COUNT(*) FROM UserRoles WHERE RoleId = 4) AS TotalStudents,
                    (SELECT COUNT(*) FROM UserRoles WHERE RoleId = 2) AS TotalTeachers,
                    (SELECT COUNT(*) FROM Classes WHERE IsActive = 1) AS TotalClasses,
                    ISNULL((
                        SELECT AVG(CAST(g.GradeTypeId AS FLOAT))
                        FROM Grades g
                        INNER JOIN Semesters s ON g.DateTime BETWEEN s.StartDate AND s.EndDate
                        WHERE g.IsActive = 1 AND s.IsActive = 1
                        AND CAST(GETDATE() AS DATE) BETWEEN s.StartDate AND s.EndDate
                    ), 0) AS AvgGradeGlobal
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_LessonSchedule] AS
                SELECT 
                    ws.Id AS ScheduleId,
                    ws.ClassId,
                    CONCAT(c.Level, c.Letter) AS ClassName,
                    ws.DayOfWeek,
                    lh.OrderNumber AS LessonNumber,
                    lh.StartTime,
                    lh.EndTime,
                    ws.SubjectId,
                    s.Name AS SubjectName,
                    cr.Name AS ClassroomName,
                    CONCAT(u.FirstName, ' ', u.LastName) AS TeacherName,
                    ws.SchoolYearId,
                    ws.SemesterId
                FROM WeeklySchedules ws
                JOIN Classes c ON ws.ClassId = c.Id
                JOIN Subjects s ON ws.SubjectId = s.Id
                JOIN Classrooms cr ON ws.ClassroomId = cr.Id
                JOIN Users u ON ws.TeacherId = u.Id
                JOIN LessonHours lh ON ws.LessonHourId = lh.Id
                WHERE ws.IsActive = 1 AND c.IsActive = 1 AND u.IsActive = 1
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_GradeDetails] AS
                SELECT 
                    g.Id AS GradeId,
                    g.StudentId,
                    g.SubjectId,
                    s.Name AS SubjectName,
                    g.GradeTypeId,
                    CAST(gt.Value AS decimal(4,2)) AS Value,
                    gt.Name AS GradeTypeName,
                    gc.Weight,
                    gc.Name AS CategoryName,
                    CONCAT(t.FirstName, ' ', t.LastName) AS TeacherName,
                    g.DateTime AS Date,
                    g.Comment,
                    1 AS SemesterId 
                FROM Grades g
                JOIN Subjects s ON g.SubjectId = s.Id
                JOIN GradeTypes gt ON g.GradeTypeId = gt.Id
                JOIN GradeCategories gc ON g.GradeCategoryId = gc.Id
                JOIN Users t ON g.TeacherId = t.Id
                WHERE g.IsActive = 1 AND s.IsActive = 1 AND t.IsActive = 1
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_AttendanceDetails] AS
                SELECT 
                    a.Id AS AttendanceId,
                    a.StudentId,
                    lh.OrderNumber AS LessonNumber,
                    s.Name AS SubjectName,
                    a.LessonId,
                    l.Date, 
                    at.Name AS StatusName,
                    at.ShortCode,
                    CAST(IIF(at.ShortCode = 'OB', 1, 0) AS BIT) AS IsPresent,
                    CAST(IIF(at.ShortCode = 'NB', 1, 0) AS BIT) AS IsAbsent,
                    CAST(IIF(at.ShortCode = 'SP', 1, 0) AS BIT) AS IsLate,
                    CAST(IIF(at.ShortCode = 'U', 1, 0) AS BIT) AS IsExcused,
                    CAST(IIF(at.ShortCode = 'NU', 1, 0) AS BIT) AS IsUnexcused
                FROM Attendances a
                JOIN Lessons l ON a.LessonId = l.Id
                JOIN Subjects s ON l.SubjectId = s.Id
                JOIN LessonHours lh ON l.LessonHourId = lh.Id
                JOIN AttendanceTypes at ON a.AttendanceTypeId = at.Id
                WHERE a.IsActive = 1 AND l.IsActive = 1
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_AnnouncementDetails] AS
                SELECT 
                    a.Id,
                    a.Title,
                    a.Description,
                    a.CreatedAt AS Date,
                    CONCAT(u.FirstName, ' ', u.LastName) AS AuthorName,
                    r.Name AS AuthorRole
                FROM Announcements a
                JOIN Users u ON a.AuthorId = u.Id
                OUTER APPLY (SELECT TOP 1 ro.Name FROM UserRoles ur JOIN Roles ro ON ur.RoleId = ro.Id WHERE ur.UserId = u.Id) r
                WHERE a.IsActive = 1 AND u.IsActive = 1
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_ClassStudentDetails] AS
                SELECT 
                    cs.ClassId,
                    u.Id AS StudentId,
                    CONCAT(u.FirstName, ' ', u.LastName) AS FullName,
                    u.Email,
                    u.Phone,
                    p.ParentName,
                    p.ParentEmail,
                    p.ParentPhone
                FROM ClassStudents cs
                JOIN Users u ON cs.StudentId = u.Id
                OUTER APPLY (
                    SELECT TOP 1 
                        CONCAT(par.FirstName, ' ', par.LastName) AS ParentName,
                        par.Email AS ParentEmail,
                        par.Phone AS ParentPhone
                    FROM ParentStudents ps
                    JOIN Users par ON ps.ParentId = par.Id
                    WHERE ps.StudentId = u.Id AND par.IsActive = 1
                ) p
                WHERE u.IsActive = 1 
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_StudentAttendanceSummary] AS
                SELECT 
                    u.Id AS StudentId,
                    u.FirstName,
                    u.LastName,
                    cs.ClassId,
                    COUNT(CASE WHEN at.ShortCode IN ('OB', 'SP') THEN 1 END) as PresentCount,
                    COUNT(CASE WHEN at.ShortCode = 'NB' THEN 1 END) as AbsentCount,
                    COUNT(CASE WHEN at.ShortCode = 'SP' THEN 1 END) as LateCount,
                    COUNT(CASE WHEN at.ShortCode = 'U' THEN 1 END) as ExcusedCount
                FROM Users u
                JOIN ClassStudents cs ON u.Id = cs.StudentId
                LEFT JOIN Attendances a ON u.Id = a.StudentId AND a.IsActive = 1
                LEFT JOIN AttendanceTypes at ON a.AttendanceTypeId = at.Id
                WHERE u.IsActive = 1 
                GROUP BY u.Id, u.FirstName, u.LastName, cs.ClassId
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_PendingExcuses] AS
                SELECT 
                    e.Id AS ExcuseId,
                    s.Id AS StudentId,
                    CONCAT(s.FirstName, ' ', s.LastName) AS StudentName,
                    CONCAT(c.Level, c.Letter) AS ClassName,
                    l.Date AS LessonDate,
                    lh.OrderNumber AS LessonNumber,
                    e.Reason,
                    CONCAT(p.FirstName, ' ', p.LastName) AS ParentName,
                    e.SubmittedAt,
                    l.TeacherId
                FROM Excuses e
                JOIN Attendances a ON e.AttendanceId = a.Id
                JOIN Lessons l ON a.LessonId = l.Id
                JOIN LessonHours lh ON l.LessonHourId = lh.Id
                JOIN Users s ON a.StudentId = s.Id
                JOIN ClassStudents cs ON s.Id = cs.StudentId AND cs.ClassId = l.ClassId
                JOIN Classes c ON l.ClassId = c.Id
                JOIN Users p ON e.ParentId = p.Id
                WHERE e.IsActive = 1 AND e.IsAccepted = 0 AND s.IsActive = 1 AND p.IsActive = 1
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_TeacherClasses] AS
                SELECT DISTINCT
                    tcs.TeacherId,
                    c.Id AS ClassId,
                    CONCAT(c.Level, c.Letter) AS ClassName,
                    sy.Id AS SchoolYearId,
                    sy.Name AS SchoolYearName,
                    (SELECT TOP 1 s.Name 
                     FROM TeacherClassSubjects tcs2 
                     JOIN Subjects s ON tcs2.SubjectId = s.Id 
                     WHERE tcs2.ClassId = c.Id AND tcs2.TeacherId = tcs.TeacherId) AS MainSubjectName
                FROM TeacherClassSubjects tcs
                JOIN Classes c ON tcs.ClassId = c.Id
                JOIN SchoolYears sy ON c.SchoolYearId = sy.Id
                WHERE c.IsActive = 1 
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_LessonSchedule]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_GradeDetails]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_AttendanceDetails]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_AnnouncementDetails]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_ClassStudentDetails]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_StudentAttendanceSummary]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_PendingExcuses]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_TeacherClasses]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_DashboardStats]");
        }
    }
}