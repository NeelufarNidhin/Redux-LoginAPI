using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReduxJWTLogin.Models;

namespace ReduxJWTLogin.Data
{
    public class DbInitializer 
    {

        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser
                {
                    UserName = "user1",
                    Email = "user1@test.com"
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "User");

                var admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@test.com"
                };

                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRolesAsync(admin, new[] { "User", "Admin" });
            }

            context.SaveChanges();

        }


    }
    }

