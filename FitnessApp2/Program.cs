using FitnessApp2.Data;
using Microsoft.EntityFrameworkCore;

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
            //DbContext Service for seeding the initial empty SQL db with some data in tables
            builder.Services.AddTransient<Seed>();

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
