using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    public partial class Functions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                create or alter function [dbo].[fn_CalculateWeightedAverage]
                (
                    @StudentId int,
                    @SubjectId int,
                    @StartDate date,
                    @EndDate date
                )
                returns decimal(4,2)
                as
                begin
                    declare @WeightedSum decimal(18,4) = 0;
                    declare @WeightTotal int = 0;

                    select 
                        @WeightedSum = sum(cast(gt.Value as decimal(18,4)) * gc.Weight),
                        @WeightTotal = sum(gc.Weight)
                    from Grades g
                    join GradeTypes gt on g.GradeTypeId = gt.Id
                    join GradeCategories gc on g.GradeCategoryId = gc.Id
                    where g.StudentId = @StudentId
                      and g.SubjectId = @SubjectId
                      and g.DateTime >= @StartDate
                      and g.DateTime <= @EndDate
                      and g.IsActive = 1;

                    if @WeightTotal = 0 return null;
                    return round(@WeightedSum / @WeightTotal, 2);
                end;
            ");

            migrationBuilder.Sql(@"
                create or alter function [dbo].[fn_GetUserRoles](@UserId int)
                returns nvarchar(max)
                as
                begin
                    declare @Result nvarchar(max);

                    select @Result = string_agg(r.Name, ', ')
                    from UserRoles ur
                    join Roles r on ur.RoleId = r.Id
                    where ur.UserId = @UserId;

                    return isnull(@Result, '');
                end;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop function if exists [dbo].[fn_CalculateWeightedAverage]");
            migrationBuilder.Sql("drop function if exists [dbo].[fn_GetUserRoles]");
        }
    }
}
