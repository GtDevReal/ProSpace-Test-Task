using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProSpaceTest.Areas.Customer.Models;
using ProSpaceTest.Data.Entity;
using ProSpaceTest.Data.Interfaces;

namespace ProSpaceTest.Areas.Customer.Controllers
{
	[Route("catalog")]
	public class CatalogController : _AreaBaseController
	{

		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public CatalogController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			return View();
		}


		[Route("products/l")]
		[HttpGet]
		public async Task<IActionResult> GetProductsList()
		{
			var models = new List<ProductViewModel>();

			try
			{
				var products = await _unitOfWork.Products.GetAllAsync();
				var currentUser = await _unitOfWork.AspNetUsers.GetCurrentUserAsync(User);

				if (currentUser != null)
				{
					if (products != null)
					{
						models = products.Select(p => _mapper.Map<ProductEntity, ProductViewModel>(p)).ToList();
						return Ok(new { data = models, customerId = currentUser.CustomerId });
					}
					return StatusCode(410, "В системе не найдено товаров!"); 
				}
				return RedirectToAction(Url.Action("Logout", "Home", new { area = "" }));
			}
			catch (Exception ex)
			{
				return BadRequest(ex.InnerException != null ? ex.InnerException : ex.Message);
			}
		}

		[Route("order/c")]
		[HttpPost]
		public async Task<IActionResult> CreateOrder([FromBody] OrderViewModel model)
		{
			if (model != null && model.Items.Count > 0)
			{
				model.OrderDate = DateOnly.FromDateTime(DateTime.Now);

				List<OrderItemEntity> entityOrderItems = model.Items.Select(i => _mapper.Map<OrderItemEntity>(i)).ToList();
				var entityOrder = _mapper.Map<OrderEntity>(model);

				try
				{
					await _unitOfWork.Orders.CreateOrderAsync(entityOrder);
					await _unitOfWork.SaveChangesAsync();
					return Ok("Заказ успешно создан!");
				}
				catch
				{
					_unitOfWork.Dispose();
					return BadRequest("При создании заказа, произошла ошибка!");
				}
			}
			return BadRequest("Данные неккоректны или их не существует!");
		}
	}
}
