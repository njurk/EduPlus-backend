using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Functions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // funkcja obliczająca średnią ważoną z przedmiotu
            migrationBuilder.Sql(@"
                CREATE OR ALTER FUNCTION [dbo].[fn_CalculateWeightedAverage]
                (
                    @StudentId INT,
                    @SubjectId INT
                )
                RETURNS DECIMAL(4, 2)
                AS
                BEGIN
                    DECLARE @Result DECIMAL(10, 2);
                    DECLARE @SumProducts DECIMAL(10, 2);
                    DECLARE @SumWeights INT;

                    SELECT 
                        @SumProducts = SUM(gt.Value * gc.Weight),
                        @SumWeights = SUM(gc.Weight)
                    FROM Grades g
                    JOIN GradeTypes gt ON g.GradeTypeId = gt.Id
                    JOIN GradeCategories gc ON g.GradeCategoryId = gc.Id
                    WHERE g.StudentId = @StudentId 
                      AND g.SubjectId = @SubjectId
                      AND g.IsActive = 1
                      AND gt.IsActive = 1
                      AND gc.IsActive = 1;

                    IF @SumWeights IS NULL OR @SumWeights = 0
                        RETURN 0.00;

                    SET @Result = @SumProducts / @SumWeights;

                    RETURN CAST(@Result AS DECIMAL(4, 2));
                END;
            ");

            // funkcja obliczania procentu frekwencji
            migrationBuilder.Sql(@"
                CREATE OR ALTER FUNCTION [dbo].[fn_CalculateAttendancePercentage]
                (
                    @StudentId INT,
                    @SubjectId INT
                )
                RETURNS DECIMAL(5, 2)
                AS
                BEGIN
                    DECLARE @TotalLessons INT;
                    DECLARE @PresentLessons INT;

                    SELECT @TotalLessons = COUNT(*)
                    FROM Attendances a
                    JOIN Lessons l ON a.LessonId = l.Id
                    WHERE a.StudentId = @StudentId
                      AND l.SubjectId = @SubjectId
                      AND a.IsActive = 1
                      AND l.IsActive = 1;

                    IF @TotalLessons = 0 RETURN 100.00;

                    SELECT @PresentLessons = COUNT(*)
                    FROM Attendances a
                    JOIN Lessons l ON a.LessonId = l.Id
                    JOIN AttendanceTypes at ON a.AttendanceTypeId = at.Id
                    WHERE a.StudentId = @StudentId
                      AND l.SubjectId = @SubjectId
                      AND a.IsActive = 1
                      AND l.IsActive = 1
                      AND at.ShortCode IN ('OB', 'SP', 'U'); 

                    RETURN CAST((CAST(@PresentLessons AS DECIMAL(10,2)) / @TotalLessons * 100) AS DECIMAL(5, 2));
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fn_CalculateWeightedAverage]");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS [dbo].[fn_CalculateAttendancePercentage]");
        }
    }
}