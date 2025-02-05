using Microsoft.AspNetCore.Identity;
using ProSpaceTest.Data.Entity;

namespace ProSpaceTest.Areas.Manager.Models
{
	public class UsersViewModel : IdentityUser
	{
		public Guid CustomerId { get; set; }
		public string Password {  get; set; }
	}
}
