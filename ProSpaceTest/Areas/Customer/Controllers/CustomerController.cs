﻿using Microsoft.AspNetCore.Mvc;

namespace ProSpaceTest.Areas.Customer.Controllers
{
	[Route("Customer")]
	public class CustomerController : _AreaBaseController
	{

		public IActionResult Index()
		{
			return View();
		}
	}
}
