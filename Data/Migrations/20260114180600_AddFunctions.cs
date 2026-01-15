using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class AddFunctions : Migration
    {
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
                RETURNS NVARCHAR(MAX)
                AS
                BEGIN
                    DECLARE @RoleNames NVARCHAR(MAX);
                    SELECT @RoleNames = STRING_AGG(r.Name, ', ')
                    FROM UserRoles ur
                    JOIN Roles r ON ur.RoleId = r.Id
                    WHERE ur.UserId = @UserId;
                    RETURN ISNULL(@RoleNames, '');
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fn_CalculateWeightedAverage]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fn_GetUserRoles]");
        }
    }
}

