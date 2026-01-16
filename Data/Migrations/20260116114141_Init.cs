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
                    ShortCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    TeacherId = table.Column<int>(type: "int", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_Targets_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                        name: "FK_Tickets_Users_UserId",
                        column: x => x.UserId,
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
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
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
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
                    table.ForeignKey(
                        name: "FK_Pages_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
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
                    table.ForeignKey(
                        name: "FK_PageContents_Users_ModifiedByUserId",
                        column: x => x.ModifiedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Excuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttendanceId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ModifiedByUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Excuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Excuses_Attendances_AttendanceId",
                        column: x => x.AttendanceId,
                        principalTable: "Attendances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Excuses_Users_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AttendanceTypes",
                columns: new[] { "Id", "CreatedAt", "IsActive", "ModifiedByUserId", "Name", "ShortCode", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Obecność", "OB", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Nieobecność", "NB", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Spóźnienie", "SP", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Usprawiedliwione", "U", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Zwolnienie", "ZW", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Classrooms",
                columns: new[] { "Id", "CreatedAt", "IsActive", "ModifiedByUserId", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "101", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "102", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "103", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "104", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "105", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "201", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "202", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "203", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "204", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "205", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "301", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "302", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "303", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "304", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "305", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 16, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "gimnastyczna 1", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 17, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "gimnastyczna 2", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 18, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "aula", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "GradeCategories",
                columns: new[] { "Id", "CreatedAt", "IsActive", "ModifiedByUserId", "Name", "UpdatedAt", "Weight" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Sprawdzian", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 3 },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Kartkówka", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Odpowiedź ustna", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Aktywność", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Zadanie domowe", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), 1 }
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
                columns: new[] { "Id", "CreatedAt", "IsActive", "ModifiedByUserId", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Zaplanowana", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Zrealizowana", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Odwołana", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Level", "ModifiedByUserId", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Najwyższy poziom uprawnień, dostęp do wszystkiego", true, 1, null, "Administrator", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zarządzanie przydzielonymi zasobami", true, 2, null, "Nauczyciel", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przeglądanie danych przypisanego użytkownika, możliwość usprawiedliwienia", true, 3, null, "Rodzic", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przeglądanie własnych danych", true, 4, null, "Uczeń", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
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
                table: "Subjects",
                columns: new[] { "Id", "CreatedAt", "IsActive", "ModifiedByUserId", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "matematyka", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "język polski", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "język angielski", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "język niemiecki", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "informatyka", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "wychowanie fizyczne", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "historia", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "WOS", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "biologia", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "chemia", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "fizyka", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "Geografia", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "przyroda", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "plastyka", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "muzyka", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 16, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "zajęcia artystyczne", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 17, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "religia", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 18, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "etyka", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 19, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "WDŻ", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 20, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "technika", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 21, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, null, "EDB", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Targets",
                columns: new[] { "Id", "CreatedAt", "Label", "ModifiedByUserId", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "WebAdmin", null, "Administrator - strona internetowa", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "WebTeacher", null, "Nauczyciel - strona internetowa", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "MobileParent", null, "Rodzic - aplikacja mobilna", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "MobileStudent", null, "Uczeń - aplikacja mobilna", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "All", null, "Wszystkie platformy", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "CreatedAt", "IsActive", "Letter", "Level", "ModifiedByUserId", "SchoolYearId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, "A", 1, null, 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), true, "C", 8, null, 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Pages",
                columns: new[] { "Id", "CreatedAt", "Link", "ModifiedByUserId", "Position", "TargetId", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "system", null, 1, 5, "Ustawienia systemu", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "dashboard", null, 1, 1, "Dashboard", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "users", null, 2, 1, "Uzytkownicy", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "announcements", null, 3, 1, "Ogloszenia", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tickets", null, 4, 1, "Zgloszenia", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "excuses", null, 5, 1, "Usprawiedliwienia", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "lessons", null, 6, 1, "Lekcje", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "schedule", null, 7, 1, "Plan lekcji", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "grades", null, 8, 1, "Oceny", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "attendance", null, 9, 1, "Frekwencja", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "classManagement", null, 10, 1, "Zarzadzanie klasami", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "settings", null, 11, 1, "Ustawienia konta", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "systemConfig", null, 12, 1, "Konfiguracja systemu", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "layout", null, 0, 1, "Pasek boczny", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "login", null, 0, 1, "Login", new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Semesters",
                columns: new[] { "Id", "CreatedAt", "EndDate", "IsActive", "ModifiedByUserId", "Name", "SchoolYearId", "StartDate", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 1, 31), true, null, "Semestr 1", 1, new DateOnly(2025, 9, 1), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 6, 30), true, null, "Semestr 2", 1, new DateOnly(2026, 2, 1), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2027, 1, 31), true, null, "Semestr 1", 2, new DateOnly(2026, 9, 1), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2027, 6, 30), true, null, "Semestr 2", 2, new DateOnly(2027, 2, 1), new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "PageContents",
                columns: new[] { "Id", "CreatedAt", "Key", "ModifiedByUserId", "PageId", "UpdatedAt", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "systemName", null, 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "EduPlus" },
                    { 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "systemLogo", null, 1, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "/assets/logo.svg" },
                    { 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Dashboard" },
                    { 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "loading", null, 2, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ladowanie..." },
                    { 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Uzytkownicy" },
                    { 6, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "actions.add", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Dodaj" },
                    { 7, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "actions.save", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zapisz" },
                    { 8, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "actions.cancel", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Anuluj" },
                    { 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "loading", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ladowanie..." },
                    { 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "empty", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Brak danych" },
                    { 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "modal.add", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Dodaj uzytkownika" },
                    { 12, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "modal.edit", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Edytuj uzytkownika" },
                    { 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.name", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Imie i nazwisko" },
                    { 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.email", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Email" },
                    { 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.roles", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Role" },
                    { 16, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.createdAt", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Utworzono" },
                    { 17, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.updatedAt", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Edytowano" },
                    { 18, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.actions", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Akcje" },
                    { 19, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.name", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Nazwisko" },
                    { 20, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.email", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Email" },
                    { 21, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.updatedAt", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Edytowano" },
                    { 22, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "confirm.delete", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Czy na pewno chcesz usunac?" },
                    { 23, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "confirm.restore", null, 3, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Czy na pewno chcesz przywrocic?" },
                    { 24, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ogloszenia" },
                    { 25, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "actions.add", null, 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Dodaj" },
                    { 26, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "loading", null, 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ladowanie..." },
                    { 27, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "empty", null, 4, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Brak ogloszen" },
                    { 28, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zgloszenia" },
                    { 29, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "loading", null, 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ladowanie..." },
                    { 30, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "empty", null, 5, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Brak zgloszen" },
                    { 31, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 6, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Usprawiedliwienia" },
                    { 32, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "loading", null, 6, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ladowanie..." },
                    { 33, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "empty", null, 6, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Brak usprawiedliwien" },
                    { 34, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 7, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Lekcje" },
                    { 35, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "loading", null, 7, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ladowanie..." },
                    { 36, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "empty", null, 7, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Brak lekcji" },
                    { 37, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 8, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Plan lekcji" },
                    { 38, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "loading", null, 8, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ladowanie planu..." },
                    { 39, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Oceny" },
                    { 40, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "actions.add", null, 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Dodaj" },
                    { 41, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "loading", null, 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ladowanie..." },
                    { 42, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "empty", null, 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Brak ocen" },
                    { 43, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.student", null, 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Uczen" },
                    { 44, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.class", null, 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Klasa" },
                    { 45, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.subject", null, 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przedmiot" },
                    { 46, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.grade", null, 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ocena" },
                    { 47, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.category", null, 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Kategoria" },
                    { 48, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.date", null, 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Data" },
                    { 49, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.student", null, 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Uczen" },
                    { 50, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.subject", null, 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przedmiot" },
                    { 51, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.date", null, 9, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Data" },
                    { 52, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Frekwencja" },
                    { 53, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "actions.add", null, 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Dodaj" },
                    { 54, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "loading", null, 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ladowanie..." },
                    { 55, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "empty", null, 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Brak danych frekwencji" },
                    { 56, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.student", null, 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Uczen" },
                    { 57, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.date", null, 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Data" },
                    { 58, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.student", null, 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Uczen" },
                    { 59, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.lesson", null, 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Lekcja" },
                    { 60, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.status", null, 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Status" },
                    { 61, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.date", null, 10, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Data" },
                    { 62, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zarzadzanie klasami" },
                    { 63, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "actions.add", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Dodaj" },
                    { 64, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "actions.save", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zapisz" },
                    { 65, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "actions.cancel", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Anuluj" },
                    { 66, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "actions.assign", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przypisz" },
                    { 67, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "loading", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ladowanie..." },
                    { 68, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "empty", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Brak klas" },
                    { 69, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "empty.students", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Brak uczniow" },
                    { 70, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "empty.subjects", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Brak przedmiotow" },
                    { 71, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "modal.newClass", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Nowa klasa" },
                    { 72, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "modal.editClass", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Edycja klasy" },
                    { 73, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "modal.assignStudents", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przypisz uczniow" },
                    { 74, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "modal.assignSubject", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przypisz przedmiot" },
                    { 75, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.students", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Uczniowie" },
                    { 76, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "tabs.subjects", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przedmioty" },
                    { 77, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "form.level", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Poziom" },
                    { 78, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "form.section", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Oddzial" },
                    { 79, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "form.subject", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przedmiot" },
                    { 80, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "form.teacher", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Nauczyciel" },
                    { 81, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "form.selectOption", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Wybierz..." },
                    { 82, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "form.selectTeacher", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Wybierz nauczyciela..." },
                    { 83, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "form.selectSubjectFirst", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Najpierw wybierz przedmiot" },
                    { 84, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "placeholder.search", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Szukaj..." },
                    { 85, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.class", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Klasa" },
                    { 86, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.studentCount", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Liczba uczniow" },
                    { 87, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.ordinal", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Lp." },
                    { 88, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.student", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Uczen" },
                    { 89, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.email", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Email" },
                    { 90, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.subject", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przedmiot" },
                    { 91, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.teacher", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Nauczyciel" },
                    { 92, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.createdAt", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Utworzono" },
                    { 93, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.updatedAt", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Edytowano" },
                    { 94, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.actions", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Akcje" },
                    { 95, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.class", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Klasa" },
                    { 96, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.lastName", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Nazwisko" },
                    { 97, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.email", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Email" },
                    { 98, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.ordinal", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Lp." },
                    { 99, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.subject", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przedmiot" },
                    { 100, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.teacher", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Nauczyciel" },
                    { 101, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.createdAt", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Utworzono" },
                    { 102, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "sort.updatedAt", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Edytowano" },
                    { 103, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "confirm.delete", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Usunac?" },
                    { 104, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "confirm.restore", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przywrocic?" },
                    { 105, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "confirm.removeStudent", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Czy na pewno chcesz usunac tego ucznia z klasy?" },
                    { 106, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "confirm.removeSubject", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Czy na pewno chcesz usunac ten przedmiot z klasy?" },
                    { 107, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "error.general", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Wystapil blad" },
                    { 108, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "error.save", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Blad zapisu" },
                    { 109, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "error.delete", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Blad usuwania" },
                    { 110, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "error.noTeachers", null, 11, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Brak nauczycieli przypisanych do tego przedmiotu." },
                    { 111, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 12, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ustawienia konta" },
                    { 112, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "section.profile", null, 12, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Profil" },
                    { 113, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "section.password", null, 12, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zmiana hasla" },
                    { 114, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "modal.new", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Nowy element" },
                    { 115, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "modal.edit", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Edycja elementu" },
                    { 116, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "actions.save", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zapisz" },
                    { 117, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "actions.cancel", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Anuluj" },
                    { 118, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "confirm.restore", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przywrocic element?" },
                    { 119, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "confirm.moveToTrash", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Przeniesc do kosza?" },
                    { 120, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "confirm.permanentDelete", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Usunac trwale?" },
                    { 121, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "error.save", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Blad zapisu" },
                    { 122, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "error.status", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Blad zmiany statusu" },
                    { 123, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "error.delete", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Blad usuwania" },
                    { 124, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.number", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Nr" },
                    { 125, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.hours", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Godziny" },
                    { 126, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.symbol", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Symbol" },
                    { 127, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.name", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Nazwa" },
                    { 128, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.value", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Wartosc" },
                    { 129, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.weight", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Waga" },
                    { 130, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.shortCode", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Skrot" },
                    { 131, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.createdAt", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Utworzono" },
                    { 132, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.updatedAt", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Edytowano" },
                    { 133, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "columns.actions", null, 13, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Akcje" },
                    { 134, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "systemName", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "EduPlus" },
                    { 135, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "version", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "v1.0.0 EduPlus" },
                    { 136, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "confirm.logout", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Na pewno chcesz sie wylogowac?" },
                    { 137, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "greeting.prefix", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Witaj, " },
                    { 138, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "greeting.suffix", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "!" },
                    { 139, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "greeting.defaultUser", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Uzytkownik" },
                    { 140, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.dashboard", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Pulpit" },
                    { 141, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.section.management", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zarzadzanie" },
                    { 142, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.users", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Uzytkownicy" },
                    { 143, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.classes", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Klasy" },
                    { 144, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.announcements", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ogloszenia" },
                    { 145, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.tickets", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zgloszenia" },
                    { 146, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.section.teaching", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Nauczanie" },
                    { 147, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.schedule", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Plany lekcji" },
                    { 148, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.lessons", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Lekcje" },
                    { 149, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.grades", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Oceny" },
                    { 150, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.attendance", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Frekwencja" },
                    { 151, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.excuses", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Usprawiedliwienia" },
                    { 152, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.section.system", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "System" },
                    { 153, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.config", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Konfiguracja" },
                    { 154, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.cms", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "CMS" },
                    { 155, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.settings", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Ustawienia" },
                    { 156, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "nav.logout", null, 14, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Wyloguj" },
                    { 157, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "title", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "EduPlus Admin" },
                    { 158, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "logoUrl", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "/logo-512.png" },
                    { 159, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "logoAlt", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "EduPlus Logo" },
                    { 160, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "form.email", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Email" },
                    { 161, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "form.password", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Haslo" },
                    { 162, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "form.forgotPassword", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zapomniałes hasla?" },
                    { 163, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "button.login", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Zaloguj sie" },
                    { 164, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "button.loading", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Logowanie..." },
                    { 165, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "error.login", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "Wystapil blad logowania" },
                    { 166, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "footer", null, 15, new DateTime(2025, 12, 27, 10, 0, 0, 0, DateTimeKind.Utc), "EduPlus v1.0.0" }
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
                name: "IX_Attendances_AttendanceTypeId",
                table: "Attendances",
                column: "AttendanceTypeId");

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
                name: "IX_Excuses_AttendanceId",
                table: "Excuses",
                column: "AttendanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Excuses_ParentId",
                table: "Excuses",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_GradeCategoryId",
                table: "Grades",
                column: "GradeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_GradeTypeId",
                table: "Grades",
                column: "GradeTypeId");

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
                name: "IX_PageContents_ModifiedByUserId",
                table: "PageContents",
                column: "ModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PageContents_PageId",
                table: "PageContents",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_ModifiedByUserId",
                table: "Pages",
                column: "ModifiedByUserId");

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
                name: "IX_Targets_ModifiedByUserId",
                table: "Targets",
                column: "ModifiedByUserId");

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
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");

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
                name: "ClassStudents");

            migrationBuilder.DropTable(
                name: "ClassSubjects");

            migrationBuilder.DropTable(
                name: "Excuses");

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
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "WeeklySchedules");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "GradeCategories");

            migrationBuilder.DropTable(
                name: "GradeTypes");

            migrationBuilder.DropTable(
                name: "Pages");

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
