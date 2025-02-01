using Microsoft.AspNetCore.Identity;

namespace ProSpaceTest.Models
{
	public class User : IdentityUser
	{
		public Guid? CustomerId { get; set; }
	}
}
