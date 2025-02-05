using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProSpaceTest.Data.Entity;
using ProSpaceTest.Data.Interfaces;
using System.Security.Claims;

namespace ProSpaceTest.Data.Repositories
{
	public class UsersRepository : IUsersRepository
	{
		private ApplicationDbContext _context;
		private readonly UserManager<UsersEntity> _userManager;
		public UsersRepository(ApplicationDbContext context, UserManager<UsersEntity> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<List<UsersEntity>> GetAllUsersAsync()
		{
			try
			{
				return await _userManager.Users.ToListAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}
		public async Task CreateUserAsync(UsersEntity user, string password)
		{
			try
			{
				await _userManager.CreateAsync(user, password);
				await _userManager.AddToRoleAsync(user, "Customer");
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task DeleteUserAsync(UsersEntity user)
		{
			try
			{
				await _userManager.DeleteAsync(user);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<UsersEntity> GetUserByIdAsync(string id)
		{
			try
			{
				return await _userManager.FindByIdAsync(id);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task UpdateUserAsync(UsersEntity user)
		{
			try
			{
				await _userManager.UpdateAsync(user);
			}
			catch (Exception)
			{
				throw;
			}
		}

		public async Task<UsersEntity> GetUserByEmailAsync(string email)
		{
			try
			{
				return await _userManager.FindByEmailAsync(email);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<UsersEntity> GetCurrentUserAsync(ClaimsPrincipal user)
		{
			return await _userManager.GetUserAsync(user);
		}
	}
}
