using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using ProSpaceTest.Models;
using System.Diagnostics;

namespace ProSpaceTest.Controllers
{
	public class HomeController : Controller
    {
		private SignInManager<User> _signInManager;

		public HomeController(SignInManager<User> signInManager)
		{
			_signInManager = signInManager;
		}

		public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Manager"))
                {
                    return RedirectToAction("Dashboard", "Manager", new { area = "Manager" });
                }
                else
                {
					return RedirectToAction("Index", "Customer", new { area = "Customer" });
				}
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
		[Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
				if (result.Succeeded)
				{
					if (User.IsInRole("Manager"))
					{
						return Ok(Url.Action("Dashboard", "Manager", new { area = "Manager" }));
					}
					else
					{
						return Ok(Url.Action("Index", "Customer", new { area = "Customer" }));
					}
				}
				else
				{
					return Unauthorized("Неверные данные!");
				}
			}
			else
			{
				return BadRequest("Данные введены некорректно!");
			}
		}

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return Ok(Url.Action("Index", "Home", new { area = "" }));
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
