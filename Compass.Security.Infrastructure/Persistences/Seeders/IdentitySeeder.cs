using System;
using Compass.Security.Domain.Commons.Enums;
using Compass.Security.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Compass.Security.Infrastructure.Persistences.Seeders
{
    public static class IdentitySeeder
    {
        public static async void Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

            if (await userManager.FindByNameAsync("admin@monkey.com") is null)
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    UserName = "admin@monkey.com",
                    Email = "admin@monkey.com",
                    EmailConfirmed = true,
                    PhoneNumber = "987654321",
                    Status = Enum.GetName(typeof(StatusEnum), StatusEnum.Active),
                    TwoFactorEnabled = false
                };

                await userManager.CreateAsync(user, "Admin2021$$");

                if (await roleManager.FindByNameAsync(Enum.GetName(typeof(RoleEnum), RoleEnum.Administrator)) is null)
                {
                    await roleManager.CreateAsync(new Role()
                    {
                        Name = Enum.GetName(typeof(RoleEnum), RoleEnum.Administrator),
                        Status = Enum.GetName(typeof(StatusEnum), StatusEnum.Active)
                    });

                    await userManager.AddToRoleAsync(user, Enum.GetName(typeof(RoleEnum), RoleEnum.Administrator));
                }
            }
        }
    }
}