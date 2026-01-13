using Data.Data.Entities;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.Metrics;
using System;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProcedures : Migration
    {
        /// <inheritdoc />
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
                CREATE OR ALTER PROCEDURE [dbo].[sp_GetWeeklyAttendance]
                AS
                BEGIN
                    SET NOCOUNT ON;
    
                    DECLARE @Today DATE = CAST(GETDATE() AS DATE);
                    DECLARE @SevenDaysAgo DATE = DATEADD(DAY, -6, @Today);

                    SELECT 
                        CAST(l.Date AS DATE) as [Date],
                        CAST(
                            ROUND(
                                (SUM(CASE WHEN at.ShortCode IN ('OB', 'SP') THEN 1.0 ELSE 0.0 END) / NULLIF(COUNT(*), 0)) * 100, 
                                0
                            ) AS INT
                        ) as [Percentage]
                    FROM Attendances a
                    JOIN Lessons l ON a.LessonId = l.Id
                    JOIN AttendanceTypes at ON a.AttendanceTypeId = at.Id
                    WHERE a.IsActive = 1 
                      AND l.Date >= @SevenDaysAgo 
                      AND l.Date <= @Today
                    GROUP BY CAST(l.Date AS DATE);
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_GenerateNextSchoolYear]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_GetWeeklyAttendance]");
        }
    }
}
