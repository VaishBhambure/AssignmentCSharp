//using LeaveManagementApp.Context;
//using LeaveManagementApp.Repository;
//using LeaveManagementApp.Services;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace LeaveManagementApp
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.
//            builder.Services.AddControllersWithViews();
//            string conn = builder.Configuration.GetConnectionString("LocaldatabaseConnection");
//            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conn));
//            // Add Identity
//            builder.Services.AddIdentity<User, IdentityRole>()
//                .AddEntityFrameworkStores<ApplicationDbContext>()
//                .AddDefaultTokenProviders();
//            builder.Services.AddScoped<IUserRepository, UserRepository>();
//            builder.Services.AddScoped<IUserService, UserService>();
//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (!app.Environment.IsDevelopment())
//            {
//                app.UseExceptionHandler("/Home/Error");
//                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//                app.UseHsts();
//            }

//            app.UseHttpsRedirection();
//            app.UseStaticFiles();

//            app.UseRouting();

//            app.UseAuthorization();

//            app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Account}/{action=Register}/{id?}");
//            using (var scope = app.Services.CreateScope())
//            {
//                var services = scope.ServiceProvider;
//                var task = DbSeeder.SeedUsers(services); // Call async method without await
//                task.GetAwaiter().GetResult(); // Ensure the task completes
//            }

//            app.Run();
//        }
//    }
//}




using LeaveManagementApp.Context;
using LeaveManagementApp.Repository;
using LeaveManagementApp.Repository.Interfaces;
using LeaveManagementApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Configure database connection
            string conn = builder.Configuration.GetConnectionString("LocaldatabaseConnection");
            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conn));

            // Add Identity with Roles
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Dependency Injection for repositories & services
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
            builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
            var app = builder.Build();

            // Ensure Roles Exist on Startup
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await EnsureRoles(services);
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Ensure authentication before authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}" // Redirects to Login instead of Register
            );
          


            // Run the app
            app.Run();
        }

        // Ensure Roles Exist
        private static async Task EnsureRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roleNames = { "Admin", "Employee" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
