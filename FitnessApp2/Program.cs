using FitnessApp2.Data;
using FitnessApp2.Interfaces;
using FitnessApp2.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace FitnessApp2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //DbContext Service from Entity for SQL Server
            builder.Services.AddDbContext<FAppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionToDbSQL"))
                );
            //DbContext Service for seeding the initial empty SQL db with some data in tables (is done one at start of the app)
            builder.Services.AddTransient<Seed>();
            //add scope for interface and repository
            builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
            builder.Services.AddScoped<IGuestRepository, GuestRepository>();
            builder.Services.AddScoped<ISectionRepository, SectionRepository>();
            builder.Services.AddScoped<IDetailRepository, DetailRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<ICourseInstructorRepository, CourseInstructorRepository>();
            builder.Services.AddScoped<ICourseGuestRepository, CourseGuestRepository>();
            //to ignore json cycles for many-to-many relationship in SQL tables
            /*
                builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            */

            var app = builder.Build();

            //injecting the service that will seed the db before the app actually starts, we do this by using commands with arguments in Terminal (PowerShell)
            if (args.Length == 1 && args[0].ToLower() == "seeddata")
            {
                SeedData(app);
            }

            void SeedData(IHost app)
            {
                var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

                using (var scope = scopedFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<Seed>();
                    service.SeedDataContext();
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
