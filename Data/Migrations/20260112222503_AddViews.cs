using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER VIEW [dbo].[vw_DashboardStatsView] AS
                SELECT
                    (SELECT COUNT(*) FROM Users WHERE IsActive = 1) AS TotalUsers,
                    (SELECT COUNT(*) FROM Users u JOIN UserRoles ur ON u.Id = ur.UserId JOIN Roles r ON ur.RoleId = r.Id WHERE r.Name = 'Uczeń' AND u.IsActive = 1) AS TotalStudents,
                    (SELECT COUNT(*) FROM Users u JOIN UserRoles ur ON u.Id = ur.UserId JOIN Roles r ON ur.RoleId = r.Id WHERE r.Name = 'Nauczyciel' AND u.IsActive = 1) AS TotalTeachers,
                    (SELECT COUNT(*) FROM Classes WHERE IsActive = 1) AS TotalClasses;
                GO
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
                GO
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
                    END AS BIT) AS IsUnassignedParent
                FROM Users u;
                GO
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_DashboardStatsView]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_ParentStudentView]");
            migrationBuilder.Sql("DROP VIEW IF EXISTS [dbo].[vw_UserListView]");
        }
    }
}
