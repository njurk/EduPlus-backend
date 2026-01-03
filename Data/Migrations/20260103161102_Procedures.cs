using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Procedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[sp_Grade_Upsert]
                (
                    @Id INT = NULL,
                    @StudentId INT,
                    @TeacherId INT,
                    @SubjectId INT,
                    @GradeTypeId INT,
                    @GradeCategoryId INT,
                    @Comment NVARCHAR(255) = NULL
                )
                AS
                BEGIN
                    SET NOCOUNT ON;
                    DECLARE @Now DATETIME = GETUTCDATE();

                    IF @Id IS NULL OR @Id = 0
                        INSERT INTO Grades (StudentId, TeacherId, SubjectId, GradeTypeId, GradeCategoryId, [DateTime], Comment, IsActive, CreatedAt, UpdatedAt)
                        VALUES (@StudentId, @TeacherId, @SubjectId, @GradeTypeId, @GradeCategoryId, @Now, @Comment, 1, @Now, @Now);
                    ELSE
                        UPDATE Grades
                        SET GradeTypeId = @GradeTypeId, GradeCategoryId = @GradeCategoryId, Comment = @Comment, TeacherId = @TeacherId, UpdatedAt = @Now
                        WHERE Id = @Id AND IsActive = 1;
                    
                    IF @Id IS NULL OR @Id = 0 SELECT SCOPE_IDENTITY() AS NewId;
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[sp_Attendance_SetStatus]
                (
                    @LessonId INT,
                    @StudentId INT,
                    @AttendanceTypeId INT
                )
                AS
                BEGIN
                    SET NOCOUNT ON;
                    DECLARE @Now DATETIME = GETUTCDATE();
                    
                    MERGE Attendances AS target
                    USING (SELECT @LessonId AS LessonId, @StudentId AS StudentId) AS source
                    ON (target.LessonId = source.LessonId AND target.StudentId = source.StudentId AND target.IsActive = 1)
                    WHEN MATCHED THEN
                        UPDATE SET AttendanceTypeId = @AttendanceTypeId, UpdatedAt = @Now
                    WHEN NOT MATCHED THEN
                        INSERT (LessonId, StudentId, AttendanceTypeId, IsActive, CreatedAt, UpdatedAt)
                        VALUES (@LessonId, @StudentId, @AttendanceTypeId, 1, @Now, @Now);
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[sp_Announcement_SoftDelete]
                (@AnnouncementId INT)
                AS
                BEGIN
                    SET NOCOUNT ON;
                    BEGIN TRANSACTION;
                    BEGIN TRY
                        UPDATE Announcements SET IsActive = 0, UpdatedAt = GETUTCDATE() WHERE Id = @AnnouncementId;
                        DELETE FROM AnnouncementReads WHERE AnnouncementId = @AnnouncementId;
                        COMMIT TRANSACTION;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION;
                        THROW;
                    END CATCH
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[sp_User_Teacher_SoftDelete]
                (@TeacherId INT)
                AS
                BEGIN
                    SET NOCOUNT ON;
                    BEGIN TRANSACTION;
                    DECLARE @Now DATETIME = GETUTCDATE();
                    BEGIN TRY
                        UPDATE Users SET IsActive = 0, UpdatedAt = @Now WHERE Id = @TeacherId;
                        DELETE FROM TeacherClassSubjects WHERE TeacherId = @TeacherId;
                        UPDATE WeeklySchedules SET IsActive = 0, UpdatedAt = @Now WHERE TeacherId = @TeacherId;
                        COMMIT TRANSACTION;
                    END TRY
                    BEGIN CATCH
                        ROLLBACK TRANSACTION;
                        THROW;
                    END CATCH
                END;
            ");

            migrationBuilder.Sql(@"
                CREATE OR ALTER PROCEDURE [dbo].[sp_InitializeLessonAttendance]
                    @LessonId INT
                AS
                BEGIN
                    SET NOCOUNT ON;
                    DECLARE @Now DATETIME = GETUTCDATE();
                    
                    INSERT INTO Attendances (LessonId, StudentId, AttendanceTypeId, IsActive, CreatedAt, UpdatedAt)
                    SELECT l.Id, cs.StudentId, 1, 1, @Now, @Now
                    FROM Lessons l
                    JOIN ClassStudents cs ON l.ClassId = cs.ClassId
                    WHERE l.Id = @LessonId AND l.IsActive = 1
                    AND NOT EXISTS (SELECT 1 FROM Attendances a WHERE a.LessonId = @LessonId AND a.IsActive = 1);
                END
            ");

            migrationBuilder.Sql(@"
               CREATE OR ALTER PROCEDURE [dbo].[sp_GenerateLessonsFromSchedule]
                    @DateStart DATE,
                    @DateEnd DATE,
                    @ClassId INT = NULL
                AS
                BEGIN
                    SET NOCOUNT ON;
                    DECLARE @CurrentDate DATE = @DateStart;
                    DECLARE @Now DATETIME = GETUTCDATE();

                    WHILE @CurrentDate <= @DateEnd
                    BEGIN
                        INSERT INTO Lessons (SubjectId, TeacherId, ClassId, ClassroomId, LessonHourId, Topic, StatusId, IsActive, [Date], CreatedAt, UpdatedAt)
                        SELECT ws.SubjectId, ws.TeacherId, ws.ClassId, ws.ClassroomId, ws.LessonHourId, 'Temat do uzupełnienia', 1, 1, @CurrentDate, @Now, @Now
                        FROM WeeklySchedules ws
                        WHERE ws.DayOfWeek = (DATEPART(dw, @CurrentDate) + @@DATEFIRST - 2) % 7 + 1
                          AND ws.IsActive = 1
                          AND (@ClassId IS NULL OR ws.ClassId = @ClassId)
                          AND NOT EXISTS (
                              SELECT 1 FROM Lessons l 
                              WHERE l.ClassId = ws.ClassId AND l.LessonHourId = ws.LessonHourId AND l.[Date] = @CurrentDate AND l.IsActive = 1
                          );
          
                        SET @CurrentDate = DATEADD(day, 1, @CurrentDate);
                    END
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_Grade_Upsert]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_Attendance_SetStatus]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_Announcement_SoftDelete]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_User_Teacher_SoftDelete]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_InitializeLessonAttendance]");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[sp_GenerateLessonsFromSchedule]");
        }
    }
}