using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProSpaceTest.Areas.Manager.Models;
using ProSpaceTest.Data.Entity;
using ProSpaceTest.Data.Interfaces;

namespace ProSpaceTest.Areas.Manager.Controllers
{
	[Route("Distributors")]
	public class DistributorsController : _AreaBaseController
	{
		private readonly IUnitOfWork _unitOfWork;
		private IMapper _mapper = null;

		public DistributorsController(IUnitOfWork unitOfWork, IMapper mapper)
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
		public async Task<IActionResult> GetCustomersList()
		{
			try
			{
				var customerList = await _unitOfWork.Customers.GetAllCustomersAsync();

				if (customerList != null)
				{
					var usersList = await _unitOfWork.AspNetUsers.GetAllUsersAsync();
					var model = customerList.Select(m => _mapper.Map<CustomerViewModel>(m)).ToList();
					var users = usersList.Select(m => _mapper.Map<UsersViewModel>(m)).ToList();
					model.ForEach(c => c.Users = users.Where(u => u.CustomerId == c.Id).ToList());

					return Ok(model);
				}
				return StatusCode(410, "В системе нету данных");

			}
			catch (Exception ex)
			{
				return BadRequest(ex.InnerException != null ? ex.InnerException : ex.Message);
			}
		}

		[Route("c")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateCustomer([FromBody] CustomerViewModel model)
		{
			if (model != null)
			{
				model.Id = Guid.NewGuid();
				model.Code = DateTime.Now.ToString("ddmm-yyyy");
				var entity = _mapper.Map<CustomerEntity>(model);
				try
				{
					await _unitOfWork.Customers.CreateCustomer(entity);
					await _unitOfWork.SaveChangesAsync();

					return Ok("Успешно");
				}
				catch (Exception ex)
				{
					_unitOfWork.Dispose();
					return BadRequest(ex.InnerException != null ? ex.InnerException : ex.Message);
				}
			}
			return BadRequest("Данные Некорректны или их не существует");
		}

		[Route("e")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditUser([FromBody] CustomerViewModel model)
		{
			if (model != null)
			{
				var entity = _mapper.Map<CustomerEntity>(model);

				try
				{
					_unitOfWork.Customers.UpdateCustomer(entity);
					await _unitOfWork.SaveChangesAsync();
					return Ok();
				}
				catch (Exception ex)
				{
					_unitOfWork.Dispose();
					return BadRequest(ex.InnerException != null ? ex.InnerException : ex.Message);
				}

			}
			return BadRequest("Данные Некорректны или их не существует!");
		}

		[Route("d/{id:guid}")]
		[HttpPost]
		public async Task<IActionResult> DeleteCustomer(Guid id)
		{
			if (id != null)
			{
				var entity = await _unitOfWork.Customers.GetCustomerByIdAsync(id);
				if (entity != null)
				{
					try
					{
						//await _unitOfWork.AspNetUsers.DeleteUsersByCustomerId(entity.Id);
						_unitOfWork.Customers.DeleteCustomer(entity);
						await _unitOfWork.SaveChangesAsync();
						return Ok();
					}
					catch (Exception e)
					{
						_unitOfWork.Dispose();
						return BadRequest(e.InnerException);
					}
				}
				return StatusCode(410, "В системе нет данных!");
			}
			return BadRequest("Данные Некорректны или их не существует");
		}

		[Route("u/c")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateUser([FromBody] UsersViewModel model)
		{
			if (model != null)
			{
				var user = await _unitOfWork.AspNetUsers.GetUserByEmailAsync(model.Email);
				if (user == null)
				{
					var entity = new UsersEntity
					{
						CustomerId = model.CustomerId,
						UserName = model.Email,
						Email = model.Email,
						EmailConfirmed = true,
						NormalizedEmail = model.Email.ToUpper(),
						NormalizedUserName = model.Email.ToUpper(),
						PhoneNumber = model.PhoneNumber,
					};
					try
					{
						await _unitOfWork.AspNetUsers.CreateUserAsync(entity, model.Password);
						await _unitOfWork.SaveChangesAsync();
						return Ok();
					}
					catch (Exception ex)
					{
						_unitOfWork.Dispose();
						return BadRequest(ex.InnerException != null ? ex.InnerException : ex.Message);
					}
				}
				return Conflict("Пользователь с таким email уже существует");

			}
			return BadRequest("Данные Некорректны или их не существует");
		}

		[Route("u/e")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditUser([FromBody] UsersViewModel model)
		{
			if (model != null)
			{
				var user = await _unitOfWork.AspNetUsers.GetUserByIdAsync(model.Id);
				if (user != null)
				{
					user.Email = model.Email;
					user.NormalizedEmail = model.Email.ToUpper();
					user.UserName = model.Email;
					user.NormalizedUserName = model.Email.ToUpper();

					user.PhoneNumber = model.PhoneNumber;

					try
					{
						await _unitOfWork.AspNetUsers.UpdateUserAsync(user);
						await _unitOfWork.SaveChangesAsync();
						return Ok();
					}
					catch (Exception ex)
					{
						_unitOfWork.Dispose();
						return BadRequest(ex.InnerException != null ? ex.InnerException : ex.Message);
					}
				}
				return BadRequest("Данные Некорректны или их не существует");

			}
			return BadRequest("Данные Некорректны или их не существует");
		}

		[Route("u/d/{id}")]
		[HttpPost]
		public async Task<IActionResult> DeleteUser(string id)
		{
			if (id != null)
			{
				var entity = await _unitOfWork.AspNetUsers.GetUserByIdAsync(id);
				if (entity != null)
				{
					try
					{
						await _unitOfWork.AspNetUsers.DeleteUserAsync(entity);
						await _unitOfWork.SaveChangesAsync();
						return Ok();
					}
					catch (Exception e)
					{
						_unitOfWork.Dispose();
						return BadRequest(e.InnerException);
					}
				}
				return StatusCode(410, "В системе нет данных!");
			}
			return BadRequest("Данные Некорректны или их не существует");
		}
	}
}
