using KhumaloCrafts_Part2.Data;
using KhumaloCrafts_Part2.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static KhumaloCrafts_Part2.Data.DbSeeder;

namespace KhumaloCrafts_Part2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services
                .AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();

           

            builder.Services.AddTransient<IHomeRepository, HomeRepository>();
            builder.Services.AddTransient<ICartRepository, CartRepository>();
            builder.Services.AddTransient<IUserOrderRepository, UserOrderRepository>();
            builder.Services.AddTransient<IStockRepository, StockRepository>();
            builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
            builder.Services.AddTransient<IFileService, FileService>();
            builder.Services.AddTransient<IProductRepository, ProductRepository>();
            builder.Services.AddTransient<IReportRepository, ReportRepository>();

            builder.Services.AddDurableClientFactory();
            builder.Services.AddHttpClient();

            var app = builder.Build();
            // Uncomment it when you run the project first time, It will registered an admin
            // using (var scope = app.Services.CreateScope())
           // {
             //   await DbSeeder.SeedDefaultData(scope.ServiceProvider);
             // }

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                DataSeeder.SeedOrderStatuses(dbContext);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                  pattern: "{controller=Home}/{action=HomeScreen}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
