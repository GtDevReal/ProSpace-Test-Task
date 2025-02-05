using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProSpaceTest.Areas.Customer.Controllers
{
	[Area("Customer")]
	[Authorize(Roles = "Customer")]
	public class _AreaBaseController : Controller
	{
	}
}
