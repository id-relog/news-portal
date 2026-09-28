using Microsoft.AspNetCore.Identity;

namespace WebAppUI.Auth
{
    public static class AdminSeeder
    {
        public static async Task EnsureDefaultAdminAsync(IServiceProvider services, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return;

            using var scope = services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser { UserName = email, Email = email };
                var createResult = await userManager.CreateAsync(user, password);
                if (!createResult.Succeeded) return;
            }

            await userManager.AddToRoleAsync(user, AppRoles.Administrator);
        }
    }
}

