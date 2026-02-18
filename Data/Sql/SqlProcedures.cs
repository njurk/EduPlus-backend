namespace Data.Sql
{
    public static class SqlProcedures
    {
        public const string DeactivateLessonAttendance = @"
            create or alter procedure [dbo].[sp_DeactivateLessonAttendance]
                @LessonId int
            as
            begin
                set nocount on;
                
                update Attendances
                set IsActive = 0, UpdatedAt = getdate()
                where LessonId = @LessonId and IsActive = 1;
                
                select @@rowcount as DeactivatedCount;
            end;";

        public const string GenerateLessonAttendance = @"
            create or alter procedure [dbo].[sp_GenerateLessonAttendance]
                @LessonId int,
                @ClassId int,
                @DefaultAttendanceSlug nvarchar(50) = 'present'
            as
            begin
                set nocount on;
                
                declare @DefaultTypeId int;
                select @DefaultTypeId = Id from AttendanceTypes where Slug = @DefaultAttendanceSlug and IsActive = 1;
                
                if @DefaultTypeId is null
                    select top 1 @DefaultTypeId = Id from AttendanceTypes where IsActive = 1 order by Id;
                
                insert into Attendances (LessonId, StudentId, AttendanceTypeId, IsActive, CreatedAt, UpdatedAt)
                select 
                    @LessonId,
                    cs.StudentId,
                    @DefaultTypeId,
                    1,
                    getdate(),
                    getdate()
                from ClassStudents cs
                where cs.ClassId = @ClassId
                and not exists (
                    select 1 from Attendances a 
                    where a.LessonId = @LessonId and a.StudentId = cs.StudentId
                );
                
                select @@rowcount as GeneratedCount;
            end;";

        public const string RecalculateClassStudentOrder = @"
            create or alter procedure [dbo].[sp_RecalculateClassStudentOrder]
                @ClassId int
            as
            begin
                set nocount on;
                
                update cs set 
                    OrderNumber = sub.NewOrder,
                    UpdatedAt = getdate()
                from ClassStudents cs
                inner join (
                    select cs2.Id, row_number() over (order by u.LastName, u.FirstName) as NewOrder
                    from ClassStudents cs2
                    inner join Users u on cs2.StudentId = u.Id
                    where cs2.ClassId = @ClassId
                ) sub on cs.Id = sub.Id;
                
                select @@rowcount as UpdatedCount;
            end;";

        public const string BulkInsertGrades = @"
            create or alter procedure [dbo].[sp_BulkInsertGrades]
                @SubjectId int,
                @GradeCategoryId int,
                @GradeColumnId int = null,
                @TeacherId int,
                @GradesJson nvarchar(max)
            as
            begin
                set nocount on;

                insert into Grades (StudentId, SubjectId, GradeTypeId, GradeCategoryId, GradeColumnId, Comment, DateTime, TeacherId, IsActive, CreatedAt, UpdatedAt)
                select
                    g.StudentId,
                    @SubjectId,
                    g.GradeTypeId,
                    @GradeCategoryId,
                    @GradeColumnId,
                    g.Comment,
                    getdate(),
                    @TeacherId,
                    1,
                    getdate(),
                    getdate()
                from openjson(@GradesJson)
                with (
                    StudentId int '$.studentId',
                    GradeTypeId int '$.gradeTypeId',
                    Comment nvarchar(255) '$.comment'
                ) g;

                select @@rowcount as InsertedCount;
            end;";

        public const string DropDeactivateLessonAttendance = "drop procedure if exists [dbo].[sp_DeactivateLessonAttendance]";
        public const string DropGenerateLessonAttendance = "drop procedure if exists [dbo].[sp_GenerateLessonAttendance]";
        public const string DropRecalculateClassStudentOrder = "drop procedure if exists [dbo].[sp_RecalculateClassStudentOrder]";
        public const string DropBulkInsertGrades = "drop procedure if exists [dbo].[sp_BulkInsertGrades]";
    }
}
