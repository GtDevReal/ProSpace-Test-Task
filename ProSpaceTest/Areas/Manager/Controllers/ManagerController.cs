using Microsoft.AspNetCore.Mvc;
using ProSpaceTest.Areas.Manager.Models;
using ProSpaceTest.Data.Interfaces;
using ProSpaceTest.Infrastructure;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace ProSpaceTest.Areas.Manager.Controllers
{
	public class ManagerController : _AreaBaseController
	{
		private readonly IUnitOfWork _unitOfWork;
		private IConverterHelper _converterHelper = null;

		public ManagerController(IUnitOfWork unitOfWork, IConverterHelper converterHelper)
		{
			_unitOfWork = unitOfWork;
			_converterHelper = converterHelper;
		}

		public IActionResult Dashboard()
		{
			return View();
		}

		[Route("dash/p")]
		[HttpGet]
		public async Task<IActionResult> GetProductsList()
		{
			var models = new List<ProductViewModel>();

			try
			{
				var products = await _unitOfWork.Products.GetAllAsync();
				if (products != null)
				{
					models = products.Select(m => _converterHelper.BuildProductViewModel(m)).ToList();
					return Ok(models);
				}
				return NoContent();
				
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

		}

		[Route("dash/u")]
		[HttpPost]
		public async Task<IActionResult> GetUsersList()
		{
			return Ok(new { data = "" });
		}

		[Route("dash/o")]
		[HttpPost]
		public async Task<IActionResult> GetOrdersList()
		{
			return Ok(new { data = "" });
		}

		//private string GenerateCode()
		//{
		//	Random random = new Random();
		//	StringBuilder sb = new StringBuilder();

		//	// Генерация XX (два случайных числа)
		//	sb.Append(random.Next(0, 10)); // Первое число
		//	sb.Append(random.Next(0, 10)); // Второе число
		//	sb.Append('-');

		//	// Генерация XXXX (четыре случайных числа)
		//	for (int i = 0; i < 4; i++)
		//	{
		//		sb.Append(random.Next(0, 10));
		//	}
		//	sb.Append('-');

		//	// Генерация YY (две случайные заглавные буквы)
		//	for (int i = 0; i < 2; i++)
		//	{
		//		char letter = (char)random.Next('A', 'Z' + 1); // Буквы от A до Z
		//		sb.Append(letter);
		//	}

		//	// Генерация XX (два случайных числа)
		//	sb.Append(random.Next(0, 10)); // Первое число
		//	sb.Append(random.Next(0, 10)); // Второе число

		//	return sb.ToString();
		//}
	}
}
