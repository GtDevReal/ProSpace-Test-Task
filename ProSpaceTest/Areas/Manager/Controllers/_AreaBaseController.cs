using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProSpaceTest.Areas.Manager.Controllers
{
	[Area("Manager")]
	[Authorize(Roles = "Manager")]
	public class _AreaBaseController : Controller
	{
	}
}
