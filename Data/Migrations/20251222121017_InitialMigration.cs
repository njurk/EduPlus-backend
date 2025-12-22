using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeeklySchedules_Classes_ClassId",
                table: "WeeklySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklySchedules_SchoolYears_SchoolYearId",
                table: "WeeklySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklySchedules_Semesters_SemesterId",
                table: "WeeklySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklySchedules_Subjects_SubjectId",
                table: "WeeklySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklySchedules_Users_TeacherId",
                table: "WeeklySchedules");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklySchedules_Classes_ClassId",
                table: "WeeklySchedules",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklySchedules_SchoolYears_SchoolYearId",
                table: "WeeklySchedules",
                column: "SchoolYearId",
                principalTable: "SchoolYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklySchedules_Semesters_SemesterId",
                table: "WeeklySchedules",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklySchedules_Subjects_SubjectId",
                table: "WeeklySchedules",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklySchedules_Users_TeacherId",
                table: "WeeklySchedules",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeeklySchedules_Classes_ClassId",
                table: "WeeklySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklySchedules_SchoolYears_SchoolYearId",
                table: "WeeklySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklySchedules_Semesters_SemesterId",
                table: "WeeklySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklySchedules_Subjects_SubjectId",
                table: "WeeklySchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklySchedules_Users_TeacherId",
                table: "WeeklySchedules");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklySchedules_Classes_ClassId",
                table: "WeeklySchedules",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklySchedules_SchoolYears_SchoolYearId",
                table: "WeeklySchedules",
                column: "SchoolYearId",
                principalTable: "SchoolYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklySchedules_Semesters_SemesterId",
                table: "WeeklySchedules",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklySchedules_Subjects_SubjectId",
                table: "WeeklySchedules",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklySchedules_Users_TeacherId",
                table: "WeeklySchedules",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
