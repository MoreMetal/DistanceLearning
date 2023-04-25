using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DistanceLearning.Data;

public class IdentitySeed
{
    public static async Task SeedAsync(AppDbContext identityContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (identityContext.Database.IsSqlServer())
        {
            identityContext.Database.Migrate();
        }

        if (await roleManager.FindByNameAsync(Authorization.Constants.Roles.ADMIN) == null)
        {
            await roleManager.CreateAsync(new IdentityRole(Authorization.Constants.Roles.ADMIN));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Constants.Roles.LECTURER));
            await roleManager.CreateAsync(new IdentityRole(Authorization.Constants.Roles.STUDENT));
        }

        if (await userManager.FindByNameAsync(Authorization.Constants.Email.ADMIN) == null)
        {
            {
                var adminUser = new IdentityUser { UserName = Authorization.Constants.Email.ADMIN, Email = Authorization.Constants.Email.ADMIN };
                var result = await userManager.CreateAsync(adminUser, Authorization.Constants.Password.DEFAULT);
                if (result.Succeeded)
                {
                    adminUser = await userManager.FindByNameAsync(Authorization.Constants.Email.ADMIN);
                    await userManager.AddToRoleAsync(adminUser, Authorization.Constants.Roles.ADMIN);
                }
            }

            {
                var lecturerUser = new IdentityUser { UserName = Authorization.Constants.Email.LECTURER, Email = Authorization.Constants.Email.LECTURER };
                var lecturerResult = await userManager.CreateAsync(lecturerUser, Authorization.Constants.Password.DEFAULT);
                if (lecturerResult.Succeeded)
                {
                    lecturerUser = await userManager.FindByNameAsync(Authorization.Constants.Email.LECTURER);
                    await userManager.AddToRoleAsync(lecturerUser, Authorization.Constants.Roles.LECTURER);
                }
            }

            {
                var studentUser = new IdentityUser { UserName = Authorization.Constants.Email.STUDENT, Email = Authorization.Constants.Email.STUDENT };
                var studentResult = await userManager.CreateAsync(studentUser, Authorization.Constants.Password.DEFAULT);
                if (studentResult.Succeeded)
                {
                    studentUser = await userManager.FindByNameAsync(Authorization.Constants.Email.STUDENT);
                    await userManager.AddToRoleAsync(studentUser, Authorization.Constants.Roles.STUDENT);
                }
            }

        }
    }
}
