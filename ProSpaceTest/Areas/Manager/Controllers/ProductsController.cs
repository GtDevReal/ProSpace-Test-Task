using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProSpaceTest.Areas.Manager.Models;
using ProSpaceTest.Data.Entity;
using ProSpaceTest.Data.Interfaces;

namespace ProSpaceTest.Areas.Manager.Controllers
{
	[Route("Products")]
	public class ProductsController : _AreaBaseController
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
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
		public async Task<IActionResult> GetProductsList()
		{
			var models = new List<ProductViewModel>();

			try
			{
				var products = await _unitOfWork.Products.GetAllAsync();
				if (products != null)
				{
					models = products.Select(m => _mapper.Map<ProductViewModel>(m)).ToList();
					return Ok(models);
				}
				return NoContent();

			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[Route("a")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AddProduct([FromBody] ProductViewModel model)
		{
			model.Id = Guid.NewGuid();
			if (model != null)
			{
				if (ModelState.IsValid)
				{
					try
					{
						var entity = _mapper.Map<ProductEntity>(model);
						await _unitOfWork.Products.CreateAsync(entity);
						await _unitOfWork.SaveChangesAsync();
						return Ok("Успешно!");
					}
					catch (Exception ex)
					{
						_unitOfWork.Dispose();
						return BadRequest(ex.Message);
					}
					
				}
				else
				{
					return ValidationProblem(ModelState);
				}
			}
			return BadRequest();
		}

		[Route("i/{id:guid}")]
		[HttpGet]
		public async Task<IActionResult> GetInfo(Guid id)
		{
			if (id != Guid.Empty)
			{
				try
				{
					var entity = await _unitOfWork.Products.GetByIdAsync(id);
					if (entity != null)
					{
						var model = _mapper.Map<ProductViewModel>(entity);
						return Ok(model);
					}
					return NoContent();
				}
				catch (Exception ex)
				{
					_unitOfWork.Dispose();
					return BadRequest(ex.Message);
				}
			}
			return BadRequest("Запрос пустой!");
		}

		[Route("e")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditProduct([FromBody] ProductViewModel model)
		{
			if (model != null)
			{
				if (ModelState.IsValid)
				{
					try
					{
						var entity = _mapper.Map<ProductEntity>(model);
						_unitOfWork.Products.Update(entity);
						await _unitOfWork.SaveChangesAsync();
						return Ok("Успешно!");
					}
					catch (Exception ex)
					{
						_unitOfWork.Dispose();
						return BadRequest(ex.Message);
					}

				}
				else
				{
					return BadRequest(ModelState);
				}
			}
			return BadRequest();
		}

		[Route("d/{id:guid}")]
		[HttpDelete]
		public async Task<IActionResult> DeleteProduct(Guid id)
		{
			if (id != null)
			{
				var entity = await _unitOfWork.Products.GetByIdAsync(id);
				if (entity != null)
				{

					try
					{
						_unitOfWork.Products.Delete(entity);
						await _unitOfWork.SaveChangesAsync();
						return Ok("Успешно!");
					}
					catch (Exception ex)
					{
						_unitOfWork.Dispose();
						return BadRequest(ex.Message);
					}
				}
				return BadRequest();
			}
			return BadRequest();
		}
	}
}
