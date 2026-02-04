using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendanceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorHex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsNegative = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classrooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GradeCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorHex = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GradeTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numeric = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LessonHours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonHours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LessonStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SchoolYears",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolYears", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Targets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Targets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TicketReasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Letter = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SchoolYearId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_SchoolYears_SchoolYearId",
                        column: x => x.SchoolYearId,
                        principalTable: "SchoolYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    SchoolYearId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Semesters_SchoolYears_SchoolYearId",
                        column: x => x.SchoolYearId,
                        principalTable: "SchoolYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    TargetId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Targets_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Targets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReasonId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_TicketReasons_ReasonId",
                        column: x => x.ReasonId,
                        principalTable: "TicketReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcements_Users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Excuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: true),
                    AcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Excuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Excuses_Users_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Excuses_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    GradeTypeId = table.Column<int>(type: "int", nullable: false),
                    GradeCategoryId = table.Column<int>(type: "int", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grades_GradeCategories_GradeCategoryId",
                        column: x => x.GradeCategoryId,
                        principalTable: "GradeCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grades_GradeTypes_GradeTypeId",
                        column: x => x.GradeTypeId,
                        principalTable: "GradeTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grades_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Grades_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grades_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ParentStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentStudents_Users_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParentStudents_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ParentStudents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectTeachers",
                columns: table => new
                {
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTeachers", x => new { x.SubjectId, x.TeacherId });
                    table.ForeignKey(
                        name: "FK_SubjectTeachers_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTeachers_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassStudents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassStudents_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassStudents_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassSubjects_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    ClassroomId = table.Column<int>(type: "int", nullable: false),
                    LessonHourId = table.Column<int>(type: "int", nullable: false),
                    Topic = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lessons_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_LessonHours_LessonHourId",
                        column: x => x.LessonHourId,
                        principalTable: "LessonHours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_LessonStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "LessonStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TeacherClassSubjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherClassSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherClassSubjects_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherClassSubjects_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherClassSubjects_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeacherClassSubjects_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WeeklySchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SchoolYearId = table.Column<int>(type: "int", nullable: false),
                    SemesterId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<int>(type: "int", nullable: false),
                    ClassroomId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    LessonHourId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklySchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklySchedules_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeeklySchedules_Classrooms_ClassroomId",
                        column: x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeeklySchedules_LessonHours_LessonHourId",
                        column: x => x.LessonHourId,
                        principalTable: "LessonHours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeeklySchedules_SchoolYears_SchoolYearId",
                        column: x => x.SchoolYearId,
                        principalTable: "SchoolYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeeklySchedules_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeeklySchedules_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeeklySchedules_Users_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageContents_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketReads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketReads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketReads_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketReads_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementReads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnnouncementId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementReads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnouncementReads_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnouncementReads_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AnnouncementTargets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnnouncementId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnnouncementTargets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnnouncementTargets_Announcements_AnnouncementId",
                        column: x => x.AnnouncementId,
                        principalTable: "Announcements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnnouncementTargets_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    AttendanceTypeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_AttendanceTypes_AttendanceTypeId",
                        column: x => x.AttendanceTypeId,
                        principalTable: "AttendanceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendances_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExcuseAttendances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExcuseId = table.Column<int>(type: "int", nullable: false),
                    AttendanceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcuseAttendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExcuseAttendances_Attendances_AttendanceId",
                        column: x => x.AttendanceId,
                        principalTable: "Attendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExcuseAttendances_Excuses_ExcuseId",
                        column: x => x.ExcuseId,
                        principalTable: "Excuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AttendanceTypes",
                columns: new[] { "Id", "ColorHex", "CreatedAt", "IsActive", "IsNegative", "ModifiedByUserId", "Name", "ShortCode", "Slug", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "#22c55e", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, false, null, "Obecność", "OB", "present", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "#ef4444", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, true, null, "Nieobecność", "NB", "absent", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "#eab308", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, true, null, "Spóźnienie", "SP", "late", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, "#8b5cf6", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, true, null, "Usprawiedliwione", "U", "excused", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, "#3b82f6", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, true, null, "Zwolnienie", "ZW", "released", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "GradeCategories",
                columns: new[] { "Id", "ColorHex", "CreatedAt", "IsActive", "ModifiedByUserId", "Name", "UpdatedAt", "Weight" },
                values: new object[,]
                {
                    { 1, "#ef4444", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Sprawdzian", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 3 },
                    { 2, "#22c55e", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Kartkówka", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { 3, "#f97316", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Odpowiedź ustna", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 4, "#3b82f6", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Aktywność", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 5, "#eab308", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Zadanie domowe", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 1 }
                });

            migrationBuilder.InsertData(
                table: "GradeTypes",
                columns: new[] { "Id", "CreatedAt", "IsActive", "ModifiedByUserId", "Name", "Numeric", "UpdatedAt", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Niedostateczny", "1", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 1.0m },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Dopuszczający", "2", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 2.0m },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Dostateczny", "3", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 3.0m },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Dobry", "4", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 4.0m },
                    { 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Bardzo dobry", "5", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 5.0m },
                    { 6, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Celujący", "6", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 6.0m }
                });

            migrationBuilder.InsertData(
                table: "LessonHours",
                columns: new[] { "Id", "CreatedAt", "EndTime", "IsActive", "ModifiedByUserId", "OrderNumber", "StartTime", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(8, 45, 0), true, null, 1, new TimeOnly(8, 0, 0), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(9, 40, 0), true, null, 2, new TimeOnly(8, 55, 0), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(10, 35, 0), true, null, 3, new TimeOnly(9, 50, 0), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(11, 30, 0), true, null, 4, new TimeOnly(10, 45, 0), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(12, 30, 0), true, null, 5, new TimeOnly(11, 45, 0), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(13, 35, 0), true, null, 6, new TimeOnly(12, 50, 0), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(14, 30, 0), true, null, 7, new TimeOnly(13, 45, 0), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(15, 25, 0), true, null, 8, new TimeOnly(14, 40, 0), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(16, 15, 0), true, null, 9, new TimeOnly(15, 30, 0), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "LessonStatuses",
                columns: new[] { "Id", "CreatedAt", "IsActive", "ModifiedByUserId", "Name", "Slug", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Zrealizowana", "completed", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Odwołana", "cancelled", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Zastępstwo", "substitute", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Level", "ModifiedByUserId", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Najwyższy poziom uprawnień", true, 1, null, "Administrator", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zarządzanie przydzielonymi klasami", true, 2, null, "Nauczyciel", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Dostęp do dziennika dziecka", true, 3, null, "Rodzic", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Dostęp do własnego dziennika", true, 4, null, "Uczeń", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "SchoolYears",
                columns: new[] { "Id", "CreatedAt", "EndDate", "IsActive", "ModifiedByUserId", "Name", "StartDate", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 6, 30), true, null, "2025/2026", new DateOnly(2025, 9, 1), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2027, 6, 30), true, null, "2026/2027", new DateOnly(2026, 9, 1), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Targets",
                columns: new[] { "Id", "CreatedAt", "Label", "ModifiedByUserId", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "WebAdmin", null, "Administrator - strona internetowa", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "WebTeacher", null, "Nauczyciel - strona internetowa", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Mobile", null, "Aplikacja mobilna", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "All", null, "Zasoby wspólne", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "TicketReasons",
                columns: new[] { "Id", "CreatedAt", "IsActive", "ModifiedByUserId", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Problem z logowaniem", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Zmiana danych osobowych", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Błąd w systemie", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Inne", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Pages",
                columns: new[] { "Id", "CreatedAt", "Link", "ModifiedByUserId", "Position", "TargetId", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "system", null, 1, 5, "System", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "dashboard", null, 1, 1, "Dashboard", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "users", null, 2, 1, "Użytkownicy", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "announcements", null, 3, 1, "Ogłoszenia", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tickets", null, 4, 1, "Zgłoszenia", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "excuses", null, 5, 1, "Usprawiedliwienia", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "lessons", null, 6, 1, "Lekcje", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "schedule", null, 7, 1, "Plan lekcji", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "grades", null, 8, 1, "Oceny", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "attendance", null, 9, 1, "Frekwencja", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "classManagement", null, 10, 1, "Zarządzanie klasami", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "settings", null, 11, 5, "Ustawienia konta", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "systemConfig", null, 12, 1, "Konfiguracja", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "layout", null, 0, 1, "Pasek boczny", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "teacherLogin", null, 0, 2, "Teacher Login", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 16, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "resetPassword", null, 0, 5, "Reset hasła", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 17, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "submitTicket", null, 0, 5, "Zgłoś problem", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 18, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "adminLogin", null, 0, 1, "Admin Login", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 19, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "subjects", null, 13, 1, "Przedmioty", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Semesters",
                columns: new[] { "Id", "CreatedAt", "EndDate", "IsActive", "ModifiedByUserId", "Name", "SchoolYearId", "StartDate", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 2, 28), true, null, "Semestr 1", 1, new DateOnly(2025, 9, 1), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 6, 30), true, null, "Semestr 2", 1, new DateOnly(2026, 3, 1), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2027, 2, 28), true, null, "Semestr 1", 2, new DateOnly(2026, 9, 1), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2027, 6, 30), true, null, "Semestr 2", 2, new DateOnly(2027, 3, 1), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "PageContents",
                columns: new[] { "Id", "CreatedAt", "Key", "ModifiedByUserId", "PageId", "UpdatedAt", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "systemName", null, 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "EduPlus" },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "pageTitle", null, 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "EduPlus - Twój e-dziennik" },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "faviconUrl", null, 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "logo-64.png" },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Pulpit" },
                    { 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "schoolYear", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Rok szkolny" },
                    { 6, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "stats.users", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Wszystkich użytkowników" },
                    { 7, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "stats.students", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Uczniów" },
                    { 8, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "stats.teachers", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Nauczycieli" },
                    { 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "stats.parents", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Rodziców" },
                    { 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "stats.classes", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Klas" },
                    { 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "quickActions.title", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Szybkie akcje" },
                    { 12, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "quickActions.manageUsers", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zarządzaj użytkownikami" },
                    { 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "quickActions.newAnnouncement", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Nowe ogłoszenie" },
                    { 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "quickActions.tickets", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zgłoszenia" },
                    { 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tickets.title", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ostatnie zgłoszenia" },
                    { 16, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tickets.viewAll", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zobacz wszystkie" },
                    { 17, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tickets.empty", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Brak zgłoszeń" },
                    { 18, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Użytkownicy" },
                    { 19, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ogłoszenia" },
                    { 20, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zgłoszenia" },
                    { 21, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 6, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Usprawiedliwienia" },
                    { 22, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 7, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Lekcje" },
                    { 23, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 8, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Plan lekcji" },
                    { 24, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "days.monday", null, 8, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Poniedziałek" },
                    { 25, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "days.tuesday", null, 8, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Wtorek" },
                    { 26, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "days.wednesday", null, 8, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Środa" },
                    { 27, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "days.thursday", null, 8, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Czwartek" },
                    { 28, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "days.friday", null, 8, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Piątek" },
                    { 29, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Oceny" },
                    { 30, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Frekwencja" },
                    { 31, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Klasy" },
                    { 32, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 12, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ustawienia konta" },
                    { 33, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "section.profile", null, 12, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Profil" },
                    { 34, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "section.password", null, 12, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zmiana hasła" },
                    { 35, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Konfiguracja" },
                    { 36, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "systemName", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "EduPlus Admin" },
                    { 37, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "version", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "v1.0.0 EduPlus" },
                    { 38, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.dashboard", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Pulpit" },
                    { 39, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.section.management", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zarządzanie" },
                    { 40, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.users", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Użytkownicy" },
                    { 41, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.classes", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Klasy" },
                    { 42, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.announcements", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ogłoszenia" },
                    { 43, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.tickets", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zgłoszenia" },
                    { 44, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.section.teaching", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Nauczanie" },
                    { 45, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.schedule", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Plan lekcji" },
                    { 46, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.lessons", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Lekcje" },
                    { 47, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.grades", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Oceny" },
                    { 48, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.attendance", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Frekwencja" },
                    { 49, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.excuses", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Usprawiedliwienia" },
                    { 50, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.section.system", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "System" },
                    { 51, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.config", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Konfiguracja" },
                    { 52, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.cms", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "CMS" },
                    { 53, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "EduPlus" },
                    { 54, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "logoUrl", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "logo-512.png" },
                    { 55, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "logoAlt", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "EduPlus" },
                    { 56, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "form.forgotPassword", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zapomniałeś hasła?" },
                    { 57, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "footer", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "EduPlus v1.0.0" },
                    { 58, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "helpLink", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Masz problem? Kliknij tutaj" },
                    { 59, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "adminLink", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Jestem administratorem" },
                    { 60, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title.request", null, 16, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Resetowanie hasła" },
                    { 61, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title.sent", null, 16, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Sprawdź swoją skrzynkę" },
                    { 62, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title.reset", null, 16, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ustaw nowe hasło" },
                    { 63, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "message.sent", null, 16, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Jeśli adres {email} istnieje w naszej bazie, za chwilę otrzymasz wiadomość z linkiem do resetowania hasła." },
                    { 64, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "message.linkExpirationTime", null, 16, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Link wygasa po 1 godzinie" },
                    { 65, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 17, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zgłoś problem" },
                    { 66, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "subtitle", null, 17, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Masz problem z logowaniem lub chcesz zgłosić inny problem? Wypełnij formularz poniżej." },
                    { 67, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "form.description", null, 17, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Opis problemu" },
                    { 68, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "form.descriptionPlaceholder", null, 17, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Opisz szczegółowo problem który napotkałeś" },
                    { 69, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "success.message", null, 17, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Twoje zgłoszenie zostało przyjęte. Odpowiedź otrzymasz na podany adres email." },
                    { 70, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 18, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "EduPlus Admin" },
                    { 71, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "logoUrl", null, 18, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "logo-512.png" },
                    { 72, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "logoAlt", null, 18, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "EduPlus" },
                    { 73, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "form.forgotPassword", null, 18, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zapomniałeś hasła?" },
                    { 74, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "footer", null, 18, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "EduPlus v1.0.0" },
                    { 75, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "helpLink", null, 18, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Masz problem? Kliknij tutaj" },
                    { 76, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "teacherLink", null, 18, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Jestem nauczycielem" },
                    { 77, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.users", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Użytkownicy" },
                    { 78, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.roles", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Role" },
                    { 79, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.relations", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Powiązania" },
                    { 80, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.schoolYears", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Rok szkolny" },
                    { 81, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.classrooms", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Sale" },
                    { 82, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.subjects", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przedmioty" },
                    { 83, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.lessonHours", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Godziny lekcyjne" },
                    { 84, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.lessonStatuses", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Statusy lekcji" },
                    { 85, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.gradeTypes", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Skala ocen" },
                    { 86, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.gradeCategories", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Kategorie ocen" },
                    { 87, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.attendance", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Frekwencja" },
                    { 88, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.ticketReasons", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Powody zgłoszeń" },
                    { 89, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 19, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przedmioty" },
                    { 90, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.students", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Uczniowie" },
                    { 91, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.subjects", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przedmioty" },
                    { 92, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.subjects", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przedmioty" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementReads_AnnouncementId_UserId",
                table: "AnnouncementReads",
                columns: new[] { "AnnouncementId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementReads_UserId",
                table: "AnnouncementReads",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_AuthorId",
                table: "Announcements",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_CreatedAt_AuthorId",
                table: "Announcements",
                columns: new[] { "CreatedAt", "AuthorId" });

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementTargets_AnnouncementId_RoleId",
                table: "AnnouncementTargets",
                columns: new[] { "AnnouncementId", "RoleId" },
                unique: true,
                filter: "[RoleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementTargets_RoleId",
                table: "AnnouncementTargets",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_AttendanceTypeId",
                table: "Attendances",
                column: "AttendanceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_IsActive_LessonId",
                table: "Attendances",
                columns: new[] { "IsActive", "LessonId" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_LessonId_StudentId",
                table: "Attendances",
                columns: new[] { "LessonId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentId",
                table: "Attendances",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SchoolYearId_Level_Letter",
                table: "Classes",
                columns: new[] { "SchoolYearId", "Level", "Letter" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudents_ClassId_StudentId",
                table: "ClassStudents",
                columns: new[] { "ClassId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClassStudents_StudentId",
                table: "ClassStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubjects_ClassId",
                table: "ClassSubjects",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassSubjects_SubjectId",
                table: "ClassSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcuseAttendances_AttendanceId",
                table: "ExcuseAttendances",
                column: "AttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcuseAttendances_ExcuseId_AttendanceId",
                table: "ExcuseAttendances",
                columns: new[] { "ExcuseId", "AttendanceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Excuses_ParentId",
                table: "Excuses",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Excuses_StudentId",
                table: "Excuses",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_GradeCategoryId",
                table: "Grades",
                column: "GradeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_GradeTypeId",
                table: "Grades",
                column: "GradeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_IsActive_CreatedAt",
                table: "Grades",
                columns: new[] { "IsActive", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Grades_StudentId_SubjectId_DateTime",
                table: "Grades",
                columns: new[] { "StudentId", "SubjectId", "DateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Grades_SubjectId",
                table: "Grades",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_TeacherId",
                table: "Grades",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ClassId",
                table: "Lessons",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ClassroomId",
                table: "Lessons",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_Date_ClassId_SubjectId",
                table: "Lessons",
                columns: new[] { "Date", "ClassId", "SubjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_LessonHourId",
                table: "Lessons",
                column: "LessonHourId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_StatusId",
                table: "Lessons",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_SubjectId",
                table: "Lessons",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TeacherId",
                table: "Lessons",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_PageContents_PageId",
                table: "PageContents",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_TargetId",
                table: "Pages",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentStudents_ParentId_StudentId",
                table: "ParentStudents",
                columns: new[] { "ParentId", "StudentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ParentStudents_StudentId",
                table: "ParentStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentStudents_UserId",
                table: "ParentStudents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_UserId",
                table: "PasswordResetTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Semesters_SchoolYearId",
                table: "Semesters",
                column: "SchoolYearId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_TeacherId",
                table: "SubjectTeachers",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherClassSubjects_ClassId",
                table: "TeacherClassSubjects",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherClassSubjects_SubjectId",
                table: "TeacherClassSubjects",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherClassSubjects_TeacherId_ClassId_SubjectId",
                table: "TeacherClassSubjects",
                columns: new[] { "TeacherId", "ClassId", "SubjectId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherClassSubjects_UserId",
                table: "TeacherClassSubjects",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketReads_TicketId_UserId",
                table: "TicketReads",
                columns: new[] { "TicketId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketReads_UserId",
                table: "TicketReads",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Email",
                table: "Tickets",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ReasonId",
                table: "Tickets",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId_RoleId",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_LastName_FirstName",
                table: "Users",
                columns: new[] { "LastName", "FirstName" });

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_ClassId",
                table: "WeeklySchedules",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_ClassroomId",
                table: "WeeklySchedules",
                column: "ClassroomId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_LessonHourId",
                table: "WeeklySchedules",
                column: "LessonHourId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_SchoolYearId",
                table: "WeeklySchedules",
                column: "SchoolYearId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_SemesterId",
                table: "WeeklySchedules",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_SubjectId",
                table: "WeeklySchedules",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_TeacherId",
                table: "WeeklySchedules",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnnouncementReads");

            migrationBuilder.DropTable(
                name: "AnnouncementTargets");

            migrationBuilder.DropTable(
                name: "ClassStudents");

            migrationBuilder.DropTable(
                name: "ClassSubjects");

            migrationBuilder.DropTable(
                name: "ExcuseAttendances");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "PageContents");

            migrationBuilder.DropTable(
                name: "ParentStudents");

            migrationBuilder.DropTable(
                name: "PasswordResetTokens");

            migrationBuilder.DropTable(
                name: "SubjectTeachers");

            migrationBuilder.DropTable(
                name: "TeacherClassSubjects");

            migrationBuilder.DropTable(
                name: "TicketReads");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "WeeklySchedules");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Excuses");

            migrationBuilder.DropTable(
                name: "GradeCategories");

            migrationBuilder.DropTable(
                name: "GradeTypes");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Semesters");

            migrationBuilder.DropTable(
                name: "AttendanceTypes");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Targets");

            migrationBuilder.DropTable(
                name: "TicketReasons");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Classrooms");

            migrationBuilder.DropTable(
                name: "LessonHours");

            migrationBuilder.DropTable(
                name: "LessonStatuses");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "SchoolYears");
        }
    }
}
