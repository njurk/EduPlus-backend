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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Target",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Target", x => x.Id);
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    TargetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Target_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Target",
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    Label = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false)
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
                name: "AnnouncementReads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnnouncementId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                name: "TicketMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TicketId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketMessages_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TicketMessages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
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
                columns: new[] { "Id", "CreatedAt", "IsActive", "Name", "ShortCode", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Obecność", "OB", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Nieobecność", "NB", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Spóźnienie", "SP", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Usprawiedliwione", "U", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Zwolnienie", "ZW", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Classrooms",
                columns: new[] { "Id", "CreatedAt", "IsActive", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "101", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "102", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "103", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "104", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "105", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "201", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "202", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "203", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "204", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "205", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "301", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "302", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "303", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "304", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "305", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 16, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "gimnastyczna 1", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 17, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "gimnastyczna 2", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 18, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "aula", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "GradeCategories",
                columns: new[] { "Id", "CreatedAt", "IsActive", "Name", "UpdatedAt", "Weight" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Sprawdzian", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), 3 },
                    { 2, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Kartkówka", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { 3, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Odpowiedź ustna", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 4, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Aktywność", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 5, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Zadanie domowe", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), 1 }
                });

            migrationBuilder.InsertData(
                table: "GradeTypes",
                columns: new[] { "Id", "CreatedAt", "IsActive", "Name", "Numeric", "UpdatedAt", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Niedostateczny", "1", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), 1.0m },
                    { 2, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Dopuszczający", "2", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), 2.0m },
                    { 3, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Dostateczny", "3", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), 3.0m },
                    { 4, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Dobry", "4", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), 4.0m },
                    { 5, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Bardzo dobry", "5", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), 5.0m },
                    { 6, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Celujący", "6", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), 6.0m }
                });

            migrationBuilder.InsertData(
                table: "LessonHours",
                columns: new[] { "Id", "CreatedAt", "EndTime", "IsActive", "OrderNumber", "StartTime", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(8, 45, 0), true, 1, new TimeOnly(8, 0, 0), new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(9, 40, 0), true, 2, new TimeOnly(8, 55, 0), new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(10, 35, 0), true, 3, new TimeOnly(9, 50, 0), new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(11, 30, 0), true, 4, new TimeOnly(10, 45, 0), new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(12, 30, 0), true, 5, new TimeOnly(11, 45, 0), new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(13, 35, 0), true, 6, new TimeOnly(12, 50, 0), new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(14, 30, 0), true, 7, new TimeOnly(13, 45, 0), new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(15, 25, 0), true, 8, new TimeOnly(14, 40, 0), new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), new TimeOnly(16, 15, 0), true, 9, new TimeOnly(15, 30, 0), new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "LessonStatuses",
                columns: new[] { "Id", "CreatedAt", "IsActive", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Zaplanowana", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Zrealizowana", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Odwołana", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "Level", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), "Najwyższy poziom uprawnień, dostęp do wszystkiego", true, 1, "Administrator", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), "Zarządzanie przydzielonymi zasobami", true, 2, "Nauczyciel", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), "Przeglądanie danych przypisanego użytkownika, możliwość usprawiedliwienia", true, 3, "Rodzic", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), "Przeglądanie własnych danych", true, 4, "Uczeń", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "SchoolYears",
                columns: new[] { "Id", "CreatedAt", "EndDate", "IsActive", "Name", "StartDate", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 6, 30), true, "2025/2026", new DateOnly(2025, 9, 1), new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2027, 6, 30), true, "2026/2027", new DateOnly(2026, 9, 1), new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "CreatedAt", "IsActive", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "matematyka", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "język polski", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "język angielski", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "język niemiecki", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "informatyka", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "wychowanie fizyczne", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "historia", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "WOS", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "biologia", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "chemia", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "fizyka", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "Geografia", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "przyroda", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "plastyka", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "muzyka", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 16, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "zajęcia artystyczne", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 17, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "religia", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 18, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "etyka", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 19, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "WDŻ", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 20, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "technika", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 21, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "EDB", new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Target",
                columns: new[] { "Id", "Label", "Title" },
                values: new object[,]
                {
                    { 1, "WebAdmin", "Administrator - strona internetowa" },
                    { 2, "WebTeacher", "Nauczyciel - strona internetowa" },
                    { 3, "MobileParent", "Rodzic - aplikacja mobilna" },
                    { 4, "MobileStudent", "Uczeń - aplikacja mobilna" }
                });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "CreatedAt", "IsActive", "Letter", "Level", "SchoolYearId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "A", 1, 1, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), true, "C", 8, 1, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Semesters",
                columns: new[] { "Id", "CreatedAt", "EndDate", "IsActive", "Name", "SchoolYearId", "StartDate", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 1, 31), true, "Semestr 1", 1, new DateOnly(2025, 9, 1), new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2026, 6, 30), true, "Semestr 2", 1, new DateOnly(2026, 2, 1), new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2027, 1, 31), true, "Semestr 1", 2, new DateOnly(2026, 9, 1), new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc), new DateOnly(2027, 6, 30), true, "Semestr 2", 2, new DateOnly(2027, 2, 1), new DateTime(2025, 12, 27, 22, 0, 0, 0, DateTimeKind.Utc) }
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
                name: "IX_Grades_StudentId",
                table: "Grades",
                column: "StudentId");

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
                name: "IX_TicketMessages_SenderId",
                table: "TicketMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketMessages_TicketId",
                table: "TicketMessages",
                column: "TicketId");

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
                name: "TeacherClassSubjects");

            migrationBuilder.DropTable(
                name: "TicketMessages");

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
                name: "Target");

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
