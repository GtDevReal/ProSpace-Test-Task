using Microsoft.AspNetCore.Mvc;

namespace ProSpaceTest.Areas.Customer.Controllers
{
	[Route("orders")]
	public class OrdersController : _AreaBaseController
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
