using KhumaloCrafts_Part2.Constants;
using Microsoft.AspNetCore.Identity;

namespace KhumaloCrafts_Part2.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefaultData(IServiceProvider service)
        {
            var userMgr = service.GetService<UserManager<IdentityUser>>();
            var roleMgr = service.GetService<RoleManager<IdentityRole>>();
            //adding some roles to db
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));

            // create admin user

            var admin = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            };

            var userInDb = await userMgr.FindByEmailAsync(admin.Email);
            if (userInDb is null)
            {
                await userMgr.CreateAsync(admin, "Admin@123");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
            }

        }


        public static class DataSeeder
        {
            public static void SeedOrderStatuses(ApplicationDbContext context)
            {
                if (!context.OrderStatuses.Any())
                {
                    context.OrderStatuses.AddRange(
                        new OrderStatus { StatusName = "Pending" },
                        new OrderStatus { StatusName = "Processing" },
                        new OrderStatus { StatusName = "Shipped" },
                        new OrderStatus { StatusName = "Delivered" }
                    );

                    context.SaveChanges();
                }
            }
        }

    }
}
