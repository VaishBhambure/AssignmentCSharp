using LeaveManagement.Context;
using LeaveManagement.Repository;
using LeaveManagement.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LeaveManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure database connection
            string conn = builder.Configuration.GetConnectionString("LocaldatabaseConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conn));
            builder.Services.AddScoped<IUserService, UserService>();  // Register Service
            builder.Services.AddScoped<IUserRepository, UserRepository>();  // Register Repository

            // Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/User/Login";  // Redirect to Login page if not authenticated
                   // options.LogoutPath = "/User/Logout"; // Redirect here on logout
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // ⬅ Authenticate first
            app.UseAuthorization();  // ⬅ Then check authorization

            // Set the default controller and action
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=User}/{action=Register}/{id?}"
            );

            app.Run();
        }
    }
}
