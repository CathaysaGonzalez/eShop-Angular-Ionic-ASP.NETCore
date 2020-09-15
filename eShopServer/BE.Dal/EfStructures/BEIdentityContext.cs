using System;
using System.Threading.Tasks;
using BE.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BE.Dal.EfStructures{

    public class BEIdentityContext : IdentityDbContext<AppUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartLine> CartLines { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Category> Categories { get; set; }

        public long ProductId { get; set; }
        public long OrderId { get; set; }
        public long CartLineId { get; set; }

        public BEIdentityContext(DbContextOptions<BEIdentityContext> options)
            : base(options)
        {
        }

        public static async Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            UserManager<AppUser> userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string usernameAdmin = configuration["Data:AdminUser:Name"];
            string emailAdmin = configuration["Data:AdminUser:Email"];
            string passwordAdmin = configuration["Data:AdminUser:Password"];
            string roleAdmin = configuration["Data:AdminUser:Role"];
            if (await userManager.FindByNameAsync(usernameAdmin) == null)
            {
                if (await roleManager.FindByNameAsync(roleAdmin) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleAdmin));
                }
                AppUser user = new AppUser
                {
                    UserName = usernameAdmin,
                    Email = emailAdmin,
                    Role= roleAdmin,
                };
                IdentityResult result = await userManager.CreateAsync(user, passwordAdmin);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleAdmin);
                }
            }
            
        }
        public static async Task CreateUserAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            UserManager<AppUser> userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string usernameUser = configuration["Data:User:Name"];
            string emailUser = configuration["Data:User:Email"];
            string passwordUser = configuration["Data:User:Password"];
            string roleUser = configuration["Data:User:Role"];
            if (await userManager.FindByNameAsync(usernameUser) == null)
            {
                if (await roleManager.FindByNameAsync(roleUser) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleUser));
                }
                AppUser user = new AppUser
                {
                    UserName = usernameUser,
                    Email = emailUser,
                    Role = roleUser
                };
                IdentityResult result = await userManager.CreateAsync(user, passwordUser);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleUser);
                }
            }
        }
    }
}
