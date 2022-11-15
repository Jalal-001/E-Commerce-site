using CommerceApp.Business.Abstract;
using Microsoft.AspNetCore.Identity;

namespace CommerceAppWebUI.Identity
{
    public static class SeedIdentity
    {
        public static async Task Seed(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ICartService cartService, IConfiguration configuration)
        {
            var roles = configuration.GetSection("Data:Roles").GetChildren().Select(x => x.Value).ToArray();

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var users = configuration.GetSection("Data:Users");

            foreach (var section in users.GetChildren())
            {
                var firstName = section.GetValue<string>("FirstName");
                var lastName = section.GetValue<string>("LastName");
                var userName = section.GetValue<string>("UserName");
                var email = section.GetValue<string>("Email");
                var password = section.GetValue<string>("Password");
                var role = section.GetValue<string>("Role");


                if (await userManager.FindByNameAsync(userName) == null)
                {
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
                    {
                        await userManager.AddToRoleAsync(user, role);
                        cartService.InitializeCart(user.Id);
                    }
                }
            }
        }
    }
}
