using Microsoft.AspNetCore.Mvc;

namespace ProSpaceTest.Areas.Manager.Controllers
{
	public class ManagerController : _AreaBaseController
	{
		public IActionResult Dashboard()
		{
			return View();
		}
	}
}
