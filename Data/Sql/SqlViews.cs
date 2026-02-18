namespace Data.Sql
{
    public static class SqlViews
    {
        public const string DashboardStats = @"
            create or alter view [dbo].[vw_DashboardStatsView] as
            select
                (select count(*) from Users where IsActive = 1) as TotalUsers,
                (select count(*) from Users u join UserRoles ur on u.Id = ur.UserId join Roles r on ur.RoleId = r.Id where r.Level = 4 and u.IsActive = 1) as TotalStudents,
                (select count(*) from Users u join UserRoles ur on u.Id = ur.UserId join Roles r on ur.RoleId = r.Id where r.Level = 2 and u.IsActive = 1) as TotalTeachers,
                (select count(*) from Classes where IsActive = 1) as TotalClasses;";

        public const string ParentStudent = @"
            create or alter view [dbo].[vw_ParentStudentView] as
            select 
                ps.Id,
                ps.ParentId,
                (p.FirstName + ' ' + p.LastName) as ParentName,
                p.Email as ParentEmail,
                ps.StudentId,
                (s.FirstName + ' ' + s.LastName) as StudentName,
                ps.CreatedAt,
                ps.UpdatedAt
            from ParentStudents ps
            join Users p on ps.ParentId = p.Id
            join Users s on ps.StudentId = s.Id;";

        public const string UserList = @"
            create or alter view [dbo].[vw_UserListView] as
            select 
                u.Id,
                u.FirstName,
                u.LastName,
                u.Email,
                u.Phone,
                u.IsActive,
                u.CreatedAt,
                u.UpdatedAt,
                [dbo].[fn_GetUserRoles](u.Id) as RoleNames,
                cast(case 
                    when exists(select 1 from UserRoles ur join Roles r on ur.RoleId = r.Id where ur.UserId = u.Id and r.Level = 3) 
                         and not exists(select 1 from ParentStudents ps where ps.ParentId = u.Id) 
                    then 1
                    when exists(select 1 from UserRoles ur join Roles r on ur.RoleId = r.Id where ur.UserId = u.Id and r.Level = 4) 
                         and not exists(select 1 from ParentStudents ps where ps.StudentId = u.Id) 
                    then 1
                    else 0 
                end as bit) as IsUnassignedRelation,
                isnull(m.FirstName + ' ' + m.LastName, 'System') as ModifiedByName
            from Users u
            left join Users m on u.ModifiedByUserId = m.Id;";

        public const string AttendanceAdmin = @"
            create or alter view [dbo].[vw_AttendanceAdmin] as
            select 
                a.Id,
                a.StudentId,
                a.AttendanceTypeId,
                a.CreatedAt,
                a.UpdatedAt,
                a.IsActive,
                s.LastName + ' ' + s.FirstName as StudentName,
                s.Email as StudentEmail,
                sub.Name as SubjectName,
                t.LastName + ' ' + t.FirstName as TeacherName,
                l.Date as LessonDate,
                lh.OrderNumber,
                cast(c.Level as varchar) + c.Letter as ClassName,
                c.Id as ClassId,
                at.Name as TypeName,
                at.ShortCode,
                at.ColorHex,
                isnull(mb.LastName + ' ' + mb.FirstName, 'System') as ModifiedByName
            from Attendances a
            join Users s on a.StudentId = s.Id
            join Lessons l on a.LessonId = l.Id
            join Subjects sub on l.SubjectId = sub.Id
            join Users t on l.TeacherId = t.Id
            join LessonHours lh on l.LessonHourId = lh.Id
            join Classes c on l.ClassId = c.Id
            join AttendanceTypes at on a.AttendanceTypeId = at.Id
            left join Users mb on a.ModifiedByUserId = mb.Id;";

        public const string GradesAdmin = @"
            create or alter view [dbo].[vw_GradesAdmin] as
            select 
                g.Id,
                g.StudentId,
                g.SubjectId,
                g.GradeTypeId,
                g.GradeCategoryId,
                g.TeacherId,
                g.Comment,
                g.CreatedAt,
                g.UpdatedAt,
                g.IsActive,
                s.LastName + ' ' + s.FirstName as StudentName,
                cast(c.Level as varchar) + c.Letter as ClassName,
                c.Id as ClassId,
                sub.Name as SubjectName,
                gt.Numeric as GradeTypeName,
                gt.Value as GradeValue,
                gc.Name as CategoryName,
                gc.ColorHex as CategoryColorHex,
                t.LastName + ' ' + t.FirstName as TeacherName,
                isnull(mb.LastName + ' ' + mb.FirstName, 'System') as ModifiedByName
            from Grades g
            join Users s on g.StudentId = s.Id
            join Subjects sub on g.SubjectId = sub.Id
            join GradeTypes gt on g.GradeTypeId = gt.Id
            join GradeCategories gc on g.GradeCategoryId = gc.Id
            join Users t on g.TeacherId = t.Id
            left join Users mb on g.ModifiedByUserId = mb.Id
            left join ClassStudents cs on cs.StudentId = g.StudentId
            left join Classes c on cs.ClassId = c.Id;";

        public const string LessonsAdmin = @"
            create or alter view [dbo].[vw_LessonsAdmin] as
            select 
                l.Id,
                l.SubjectId,
                sub.Name as SubjectName,
                l.ClassId,
                cast(c.Level as varchar) + c.Letter as ClassName,
                l.TeacherId,
                t.LastName + ' ' + t.FirstName as TeacherName,
                l.ClassroomId,
                cr.Name as ClassroomName,
                l.LessonHourId,
                lh.OrderNumber,
                lh.StartTime,
                lh.EndTime,
                l.Date,
                l.Topic,
                l.StatusId,
                ls.Name as StatusName,
                l.CreatedAt,
                l.UpdatedAt,
                l.IsActive,
                isnull(mb.LastName + ' ' + mb.FirstName, 'System') as ModifiedByName
            from Lessons l
            join Subjects sub on l.SubjectId = sub.Id
            join Classes c on l.ClassId = c.Id
            join Users t on l.TeacherId = t.Id
            left join Classrooms cr on l.ClassroomId = cr.Id
            join LessonHours lh on l.LessonHourId = lh.Id
            left join LessonStatuses ls on l.StatusId = ls.Id
            left join Users mb on l.ModifiedByUserId = mb.Id;";

        public const string TeacherAssignments = @"
            create or alter view [dbo].[vw_TeacherAssignments] as
            select distinct
                tcs.TeacherId,
                tcs.ClassId,
                cast(c.Level as nvarchar(10)) + c.Letter as ClassName,
                tcs.SubjectId,
                sub.Name as SubjectName
            from TeacherClassSubjects tcs
            join Classes c on tcs.ClassId = c.Id
            join Subjects sub on tcs.SubjectId = sub.Id
            where tcs.IsActive = 1 and c.IsActive = 1;";

        public const string DropDashboardStats = "drop view if exists [dbo].[vw_DashboardStatsView]";
        public const string DropParentStudent = "drop view if exists [dbo].[vw_ParentStudentView]";
        public const string DropUserList = "drop view if exists [dbo].[vw_UserListView]";
        public const string DropAttendanceAdmin = "drop view if exists [dbo].[vw_AttendanceAdmin]";
        public const string DropGradesAdmin = "drop view if exists [dbo].[vw_GradesAdmin]";
        public const string DropLessonsAdmin = "drop view if exists [dbo].[vw_LessonsAdmin]";
        public const string DropTeacherAssignments = "drop view if exists [dbo].[vw_TeacherAssignments]";
    }
}
