namespace Data.Sql
{
    public static class SqlProcedures
    {
        public const string GenerateNextSchoolYear = @"
            create or alter procedure [dbo].[sp_GenerateNextSchoolYear]
            as
            begin
                set nocount on;

                declare @LastEndDate date;
                declare @NewStartDate date;
                declare @NewEndDate date;
                declare @NewYearName nvarchar(20);
                declare @NewYearId int;

                select top 1 @LastEndDate = EndDate from SchoolYears order by EndDate desc;

                if @LastEndDate is null set @LastEndDate = '2023-08-31';

                set @NewStartDate = dateadd(day, 1, @LastEndDate);
                set @NewEndDate = dateadd(year, 1, @LastEndDate);
                set @NewYearName = concat(year(@NewStartDate), '/', year(@NewEndDate));

                insert into SchoolYears (Name, StartDate, EndDate, IsActive, CreatedAt, UpdatedAt)
                values (@NewYearName, @NewStartDate, @NewEndDate, 0, getdate(), getdate());

                set @NewYearId = scope_identity();

                insert into Semesters (Name, StartDate, EndDate, IsActive, CreatedAt, UpdatedAt, SchoolYearId)
                values ('Semestr 1', @NewStartDate, datefromparts(year(@NewEndDate), 1, 31), 1, getdate(), getdate(), @NewYearId);

                insert into Semesters (Name, StartDate, EndDate, IsActive, CreatedAt, UpdatedAt, SchoolYearId)
                values ('Semestr 2', datefromparts(year(@NewEndDate), 2, 1), @NewEndDate, 1, getdate(), getdate(), @NewYearId);
            end;";

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

        public const string DropGenerateNextSchoolYear = "drop procedure if exists [dbo].[sp_GenerateNextSchoolYear]";
        public const string DropDeactivateLessonAttendance = "drop procedure if exists [dbo].[sp_DeactivateLessonAttendance]";
        public const string DropGenerateLessonAttendance = "drop procedure if exists [dbo].[sp_GenerateLessonAttendance]";
        public const string DropRecalculateClassStudentOrder = "drop procedure if exists [dbo].[sp_RecalculateClassStudentOrder]";
    }
}
