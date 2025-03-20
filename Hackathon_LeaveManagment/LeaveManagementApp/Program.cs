using LeaveManagementApp.Context;
using LeaveManagementApp.Repository;
using LeaveManagementApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            string conn = builder.Configuration.GetConnectionString("LocaldatabaseConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conn));
            // Add Identity
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
     name: "default",
     pattern: "{controller=Account}/{action=Register}/{id?}");
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var task = DbSeeder.SeedUsers(services); // Call async method without await
                task.GetAwaiter().GetResult(); // Ensure the task completes
            }

            app.Run();
        }
    }
}