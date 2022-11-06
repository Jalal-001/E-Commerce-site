using Microsoft.AspNetCore.Identity;

namespace CommerceAppWebUI.Identity
{
    public static class SeedIdentity
    {
        public static async Task Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            var firstName = configuration["Data:AdminUser:FirstName"];
            var lastName = configuration["Data:AdminUser:LastName"];
            var userName = configuration["Data:AdminUser:UserName"];
            var email = configuration["Data:AdminUser:Email"];
            var password = configuration["Data:AdminUser:Password"];
            var role = configuration["Data:AdminUser:Role"];

            if (await userManager.FindByNameAsync(userName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(role));

                var user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = userName,
                    Email = email,
                    EmailConfirmed = true,
                };
                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
