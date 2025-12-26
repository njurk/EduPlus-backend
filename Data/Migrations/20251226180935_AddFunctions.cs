using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFunctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Letter", "Level" },
                values: new object[] { "C", 8 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Letter", "Level" },
                values: new object[] { "B", 4 });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "IsActive", "Letter", "Level", "SchoolYearId" },
                values: new object[] { 3, true, "C", 8, 1 });
        }
    }
}
