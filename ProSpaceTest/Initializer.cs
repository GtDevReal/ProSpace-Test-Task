using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProSpaceTest.Models;
using System.Data;

namespace ProSpaceTest
{
	public static class Initializer
	{
		public static async Task InitializeRoles(IServiceProvider serviceProvider)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

			string[] roles = { "Manager", "Customer" };

			foreach (var role in roles)
			{
				if (!await roleManager.RoleExistsAsync(role))
				{
					await roleManager.CreateAsync(new IdentityRole(role));
				}
			}

			var adminEmail = "admin@admin.ru";
			var adminPassword = "Admin_123";
			if (await userManager.FindByEmailAsync(adminEmail) == null)
			{
				var admin = new User { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true, NormalizedEmail = adminEmail.ToUpper(), NormalizedUserName = adminEmail.ToUpper() };
				IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(admin, "Manager"); 
				}
			}
		}
	}
}
