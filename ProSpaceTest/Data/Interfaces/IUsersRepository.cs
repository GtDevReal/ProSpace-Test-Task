using Microsoft.AspNetCore.Identity;
using ProSpaceTest.Areas.Manager.Models;
using ProSpaceTest.Data.Entity;
using System.Drawing;
using System.Security.Claims;

namespace ProSpaceTest.Data.Interfaces
{
	public interface IUsersRepository
	{
		Task<List<UsersEntity>> GetAllUsersAsync();
		Task CreateUserAsync(UsersEntity user, string password);
		Task DeleteUserAsync(UsersEntity user);
		Task<UsersEntity> GetUserByIdAsync(string id);
		Task UpdateUserAsync(UsersEntity user);
		Task<UsersEntity> GetUserByEmailAsync(string email);
		Task<UsersEntity> GetCurrentUserAsync(ClaimsPrincipal user);
	}
}
