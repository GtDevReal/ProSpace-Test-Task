using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProSpaceTest.Areas.Manager.Models;
using ProSpaceTest.Data.Entity;
using ProSpaceTest.Data.Interfaces;

namespace ProSpaceTest.Areas.Manager.Controllers
{
	[Route("Manage")]
	public class ManageController : _AreaBaseController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ManageController(IUnitOfWork unitOfWork, IMapper mapper)
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
				var ordersEntity = await _unitOfWork.Orders.GetAllOrdersAsync();
				if (ordersEntity != null)
				{
					var ordersViewModel = ordersEntity.Select(o => _mapper.Map<OrderViewModel>(o)).ToList();
					return Ok(ordersViewModel);
				}
				return StatusCode(410, "В системе не найдено заказов!");
			}
			catch (Exception e)
			{
				return BadRequest(e.InnerException != null ? e.InnerException : e.Message);
			}
		}

		[Route("e")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateOrder([FromBody] OrderViewModel model)
		{
			if (model != null)
			{
				try
				{
					var entity = _mapper.Map<OrderEntity>(model);
					_unitOfWork.Orders.UpdateOrder(entity);
					await _unitOfWork.SaveChangesAsync();
					return Ok("Заказ успешно изменен!");
				}
				catch (Exception e)
				{
					return BadRequest(e.InnerException != null ? e.InnerException : e.Message);
				}
			}
			return BadRequest("Ошибка получения данных");
		}
	}
}
