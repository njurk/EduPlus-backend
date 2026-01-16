using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class Functions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER FUNCTION [dbo].[fn_CalculateWeightedAverage]
                (
                    @StudentId INT,
                    @SubjectId INT,
                    @StartDate DATE,
                    @EndDate DATE
                )
                RETURNS DECIMAL(4,2)
                AS
                BEGIN
                    DECLARE @WeightedSum DECIMAL(18,4) = 0;
                    DECLARE @WeightTotal INT = 0;

                    SELECT 
                        @WeightedSum = SUM(CAST(gt.Value AS DECIMAL(18,4)) * gc.Weight),
                        @WeightTotal = SUM(gc.Weight)
                    FROM Grades g
                    JOIN GradeTypes gt ON g.GradeTypeId = gt.Id
                    JOIN GradeCategories gc ON g.GradeCategoryId = gc.Id
                    WHERE g.StudentId = @StudentId
                      AND g.SubjectId = @SubjectId
                      AND g.DateTime >= @StartDate
                      AND g.DateTime <= @EndDate
                      AND g.IsActive = 1;

                    IF @WeightTotal = 0 RETURN NULL;
                    RETURN ROUND(@WeightedSum / @WeightTotal, 2);
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER FUNCTION [dbo].[fn_GetUserRoles](@UserId INT)
                RETURNS NVARCHAR(MAX)
                AS
                BEGIN
                    DECLARE @Result NVARCHAR(MAX);

                    SELECT @Result = STRING_AGG(r.Name, ', ')
                    FROM UserRoles ur
                    JOIN Roles r ON ur.RoleId = r.Id
                    WHERE ur.UserId = @UserId;

                    RETURN ISNULL(@Result, '');
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
