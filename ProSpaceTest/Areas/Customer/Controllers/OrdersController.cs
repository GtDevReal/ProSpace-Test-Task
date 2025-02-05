using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProSpaceTest.Areas.Customer.Models;
using ProSpaceTest.Data.Interfaces;

namespace ProSpaceTest.Areas.Customer.Controllers
{
	[Route("orders")]
	public class OrdersController : _AreaBaseController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public OrdersController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public IActionResult Index()
		{
			return View();
		}

		[Route("l")]
		[HttpGet]
		public async Task<IActionResult> GetOrdersList()
		{
			try
			{
				var user = await _unitOfWork.AspNetUsers.GetCurrentUserAsync(User);
				if (user != null)
				{
					var ordersEntity = await _unitOfWork.Orders.GetAllOrdersByCustomerIdAsync(user.CustomerId);
					if (ordersEntity != null)
					{
						var ordersViewModel = ordersEntity.Select(o => _mapper.Map<OrderViewModel>(o)).ToList();
						return Ok(ordersViewModel);
					}
					return StatusCode(410, "В системе не найдено заказов для вашей организации!");
				}
				return RedirectToAction("Index", "Home", new { area = "" });
			}
			catch (Exception e)
			{
				return BadRequest(e.InnerException != null ? e.InnerException : e.Message);
			}
		}

		[Route("d/{id:guid}")]
		[HttpPost]
		public async Task<IActionResult> DeleteOrder(Guid id)
		{
			if (id != null)
			{
				try
				{
					var order = await _unitOfWork.Orders.GetOrderByIdAsync(id);
					if (order != null)
					{
						_unitOfWork.Orders.DeleteOrder(order);
						await _unitOfWork.SaveChangesAsync();
						return Ok("Заказ успешно удален!");

					}
					return StatusCode(410, "Такого заказа не существует!");
				}
				catch (Exception e)
				{
					return BadRequest(e.InnerException != null ? e.InnerException : e.Message);
				}
			}
			return BadRequest("Заказ с таким Id не найден!");
		}
	}
}
