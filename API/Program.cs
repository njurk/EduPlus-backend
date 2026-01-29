using API.Middleware;
using BusinessLogic.Interceptors;
using BusinessLogic.Seeders;
using BusinessLogic.Services;
using Data.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<AuditSaveChangesInterceptor>();

            builder.Services.AddDbContext<EduPlusDbContext>((sp, options) =>
            {
                var interceptor = sp.GetRequiredService<AuditSaveChangesInterceptor>();
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                       .AddInterceptors(interceptor);
            });

            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();
            builder.Services.AddScoped<IAttendanceTypeService, AttendanceTypeService>();
            builder.Services.AddScoped<IClassService, ClassService>();
            builder.Services.AddScoped<IClassroomService, ClassroomService>();
            builder.Services.AddScoped<IAnnouncementService, AnnouncementService>();
            builder.Services.AddScoped<IAnnouncementReadService, AnnouncementReadService>();
            builder.Services.AddScoped<ITicketReadService, TicketReadService>();
            builder.Services.AddScoped<IAttendanceService, AttendanceService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IPasswordResetService, PasswordResetService>();
            builder.Services.AddScoped<IPageContentService, PageContentService>();
            builder.Services.AddScoped<IWeeklyScheduleService, WeeklyScheduleService>();
            builder.Services.AddScoped<ILessonService, LessonService>();
            builder.Services.AddScoped<IExcuseService, ExcuseService>();
            builder.Services.AddScoped<IExportService, ExportService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IGradeService, GradeService>();
            builder.Services.AddScoped<IGradeTypeService, GradeTypeService>();
            builder.Services.AddScoped<IGradeCategoryService, GradeCategoryService>();
            builder.Services.AddScoped<ILessonHourService, LessonHourService>();
            builder.Services.AddScoped<ILessonStatusService, LessonStatusService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            builder.Services.AddScoped<ISchoolYearService, SchoolYearService>();
            builder.Services.AddScoped<ISemesterService, SemesterService>();
            builder.Services.AddScoped<IParentStudentService, ParentStudentService>();
            builder.Services.AddScoped<IPageService, PageService>();
            builder.Services.AddScoped<IUserRoleService, UserRoleService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<ILayoutService, LayoutService>();
            builder.Services.AddScoped<ITargetService, TargetService>();
            builder.Services.AddSingleton<IEventLogService, EventLogService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Allow",
                    policy => policy
                        .WithOrigins("http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                    );
            });

            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<EduPlusDbContext>();
                    var passwordHashService = services.GetRequiredService<IPasswordHashService>();

                    context.Database.Migrate();
                    context.ApplySqlObjects();

                    var seeder = new DataSeeder(context);

                    seeder.Seed(passwordHashService);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Error w DataSeeder.cs");
                }
            }

            app.UseMiddleware<ErrorLoggingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("Allow");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
