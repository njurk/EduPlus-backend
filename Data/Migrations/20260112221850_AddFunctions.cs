using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER FUNCTION [dbo].[fn_CalculateWeightedAverage]
                (@StudentId INT, @SubjectId INT, @StartDate DATETIME, @EndDate DATETIME)
                RETURNS DECIMAL(4, 2)
                AS
                BEGIN
                    RETURN (
                        SELECT ISNULL(CAST(SUM(gt.Value * gc.Weight) / NULLIF(SUM(gc.Weight), 0) AS DECIMAL(4, 2)), 0.00)
                        FROM Grades g
                        JOIN GradeTypes gt ON g.GradeTypeId = gt.Id
                        JOIN GradeCategories gc ON g.GradeCategoryId = gc.Id
                        WHERE g.StudentId = @StudentId 
                          AND g.SubjectId = @SubjectId
                          AND g.IsActive = 1
                          AND gt.IsActive = 1
                          AND gc.IsActive = 1
                          AND g.CreatedAt >= @StartDate AND g.CreatedAt <= @EndDate
                    );
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER FUNCTION [dbo].[fn_GetUserRoles](@UserId INT)
                RETURNS NVARCHAR(MAX) AS
                BEGIN
                    RETURN (SELECT STRING_AGG(r.Name, ', ') FROM UserRoles ur JOIN Roles r ON ur.RoleId = r.Id WHERE ur.UserId = @UserId);
                END;
                GO
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fn_CalculateWeightedAverage]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fn_GetUserRoles]");
        }
    }
}
