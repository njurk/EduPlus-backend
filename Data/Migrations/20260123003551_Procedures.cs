using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class Procedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[sp_GenerateNextSchoolYear]
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @LastEndDate DATE;
                    DECLARE @NewStartDate DATE;
                    DECLARE @NewEndDate DATE;
                    DECLARE @NewYearName NVARCHAR(20);
                    DECLARE @NewYearId INT;

                    SELECT TOP 1 @LastEndDate = EndDate FROM SchoolYears ORDER BY EndDate DESC;

                    IF @LastEndDate IS NULL SET @LastEndDate = '2023-08-31';

                    SET @NewStartDate = DATEADD(day, 1, @LastEndDate);
                    SET @NewEndDate = DATEADD(year, 1, @LastEndDate);
                    SET @NewYearName = CONCAT(YEAR(@NewStartDate), '/', YEAR(@NewEndDate));

                    INSERT INTO SchoolYears (Name, StartDate, EndDate, IsActive, CreatedAt, UpdatedAt)
                    VALUES (@NewYearName, @NewStartDate, @NewEndDate, 0, GETDATE(), GETDATE());

                    SET @NewYearId = SCOPE_IDENTITY();

                    INSERT INTO Semesters (Name, StartDate, EndDate, IsActive, CreatedAt, UpdatedAt, SchoolYearId)
                    VALUES ('Semestr 1', @NewStartDate, DATEFROMPARTS(YEAR(@NewEndDate), 1, 31), 1, GETDATE(), GETDATE(), @NewYearId);

                    INSERT INTO Semesters (Name, StartDate, EndDate, IsActive, CreatedAt, UpdatedAt, SchoolYearId)
                    VALUES ('Semestr 2', DATEFROMPARTS(YEAR(@NewEndDate), 2, 1), @NewEndDate, 1, GETDATE(), GETDATE(), @NewYearId);
                END;
            ");
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[sp_DeactivateLessonAttendance]
                    @LessonId INT
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    UPDATE Attendances
                    SET IsActive = 0, UpdatedAt = GETDATE()
                    WHERE LessonId = @LessonId AND IsActive = 1;
                    
                    SELECT @@ROWCOUNT AS DeactivatedCount;
                END;
            ");
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[sp_GenerateLessonAttendance]
                    @LessonId INT,
                    @ClassId INT,
                    @DefaultAttendanceSlug NVARCHAR(50) = 'present'
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    DECLARE @DefaultTypeId INT;
                    SELECT @DefaultTypeId = Id FROM AttendanceTypes WHERE Slug = @DefaultAttendanceSlug AND IsActive = 1;
                    
                    IF @DefaultTypeId IS NULL
                        SELECT TOP 1 @DefaultTypeId = Id FROM AttendanceTypes WHERE IsActive = 1 ORDER BY Id;
                    
                    INSERT INTO Attendances (LessonId, StudentId, AttendanceTypeId, IsActive, CreatedAt, UpdatedAt)
                    SELECT 
                        @LessonId,
                        cs.StudentId,
                        @DefaultTypeId,
                        1,
                        GETDATE(),
                        GETDATE()
                    FROM ClassStudents cs
                    WHERE cs.ClassId = @ClassId
                    AND NOT EXISTS (
                        SELECT 1 FROM Attendances a 
                        WHERE a.LessonId = @LessonId AND a.StudentId = cs.StudentId
                    );
                    
                    SELECT @@ROWCOUNT AS GeneratedCount;
                END;
            ");
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[sp_RecalculateClassStudentOrder]
                    @ClassId INT
                AS
                BEGIN
                    SET NOCOUNT ON;
                    
                    UPDATE cs SET 
                        OrderNumber = sub.NewOrder,
                        UpdatedAt = GETDATE()
                    FROM ClassStudents cs
                    INNER JOIN (
                        SELECT cs2.Id, ROW_NUMBER() OVER (ORDER BY u.LastName, u.FirstName) AS NewOrder
                        FROM ClassStudents cs2
                        INNER JOIN Users u ON cs2.StudentId = u.Id
                        WHERE cs2.ClassId = @ClassId
                    ) sub ON cs.Id = sub.Id;
                    
                    SELECT @@ROWCOUNT AS UpdatedCount;
                END;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_RecalculateClassStudentOrder]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_GenerateLessonAttendance]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_DeactivateLessonAttendance]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_GenerateNextSchoolYear]");
        }
    }
}
