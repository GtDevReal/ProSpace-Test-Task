using Microsoft.AspNetCore.Identity;

namespace ProSpaceTest.Data.Entity
{
	public class UsersEntity : IdentityUser
	{
		public Guid? CustomerId { get; set; }
		public CustomerEntity Customer { get; set; }
	}
}
