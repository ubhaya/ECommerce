using ECommerce.Server.Data;
using ECommerce.Server.Models;
using ECommerce.Shared.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ECommerce.Server.Service
{
    public class SeedData
    {
        internal static async Task EnsureSeedData(string connectionString, string userPw)
        {
            var services = new ServiceCollection();
            services.AddLogging();

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(connectionString));

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

                    context.Database.Migrate();
                    Log.Warning($"Password is {userPw}");
                    var adminId = await EnsureUser(scope.ServiceProvider, userPw, "ubhaya123@gmail.com");
                    await EnsureRole(scope.ServiceProvider, adminId, Roles.Administrator);
                }
            }
        }

        private static async Task EnsureRole(IServiceProvider serviceProvider, string uId, IEnumerable<string> roles)
        {
            var _roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            var _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            if (_roleManager == null)
                throw new NullReferenceException("roleManaher null");

            var user = await _userManager.FindByIdAsync(uId);

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                    Log.Information($"{role} is created!");
                }

                await _userManager.AddToRoleAsync(user, role);
            }
        }

        private static async Task EnsureRole(IServiceProvider serviceProvider, string uId, string role)
        {
            var roles = new List<string>
            {
                role
            };

            await EnsureRole(serviceProvider, uId, roles);
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string userPw, string email)
        {
            var _userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                var emailAddress = new MailAddress(email);
                user = new ApplicationUser
                {
                    UserName = emailAddress.User,
                    Email = email,
                    EmailConfirmed = true,
                };

                await _userManager.CreateAsync(user, userPw);
            }

            return user.Id;
        }
    }
}
