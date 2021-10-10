using System;
using Compass.Security.Application.Commons.Interfaces;
using Compass.Security.Domain.Enums;
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
            
            var userNotificationRepository = scope.ServiceProvider.GetRequiredService<IUserNotificationRepository>();

            if (await userManager.FindByNameAsync("admin@compass.com") is not null) 
                return;
            
            var user = new User()
            {
                Id = Guid.NewGuid(),
                UserName = "admin@compass.com",
                Email = "admin@compass.com",
                EmailConfirmed = true,
                PhoneNumber = "987654321",
                Status = Enum.GetName(typeof(StatusEnum), StatusEnum.Active),
                TwoFactorEnabled = false
            };

            await userManager.CreateAsync(user, "Admin2021$$");

            if (await roleManager.FindByNameAsync(Enum.GetName(typeof(RoleEnum), RoleEnum.Administrator)) is not null)
                return;
            
            await roleManager.CreateAsync(new Role()
            {
                Name = Enum.GetName(typeof(RoleEnum), RoleEnum.Administrator),
                Status = Enum.GetName(typeof(StatusEnum), StatusEnum.Active)
            });
            
            await roleManager.CreateAsync(new Role()
            {
                Name = Enum.GetName(typeof(RoleEnum), RoleEnum.Guest),
                Status = Enum.GetName(typeof(StatusEnum), StatusEnum.Active)
            });

            await userManager.AddToRoleAsync(user, Enum.GetName(typeof(RoleEnum), RoleEnum.Administrator));
            
            foreach (var type in (NotificationTypeEnum[]) Enum.GetValues(typeof(NotificationTypeEnum)))
            {
                if (!type.Equals(NotificationTypeEnum.None))
                {
                    await userNotificationRepository.InsertAsync(new UserNotification
                    {
                        UserId = user.Id,
                        Type = type,
                        Counter = 0,
                    });
                }
            }
        }
    }
}