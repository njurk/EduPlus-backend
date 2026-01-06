using Microsoft.EntityFrameworkCore;
using Data.Data;
using BusinessLogic.Services;
using System.Text.Json.Serialization;
using BusinessLogic.Seeders;
using BusinessLogic.Converters;
namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SchoolDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
            });
                
            
            builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Allow",
                    policy => policy
                        .WithOrigins("http://localhost:5173", "http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SchoolDbContext>();
                    var passwordHashService = services.GetRequiredService<IPasswordHashService>();

                    context.Database.Migrate();

                    var seeder = new DataSeeder(context);

                    seeder.Seed(passwordHashService);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Error w DataSeeder.cs");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("Allow");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
