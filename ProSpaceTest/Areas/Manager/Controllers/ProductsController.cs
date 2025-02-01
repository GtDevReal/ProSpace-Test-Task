using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProSpaceTest.Areas.Manager.Models;
using ProSpaceTest.Data.Interfaces;
using ProSpaceTest.Data.Repositories;
using ProSpaceTest.Infrastructure;

namespace ProSpaceTest.Areas.Manager.Controllers
{
	public class ProductsController : _AreaBaseController
	{
		private readonly IUnitOfWork _unitOfWork;
		private IConverterHelper _converterHelper = null;

		public ProductsController(IUnitOfWork unitOfWork, IConverterHelper converterHelper)
		{
			_unitOfWork = unitOfWork;
			_converterHelper = converterHelper;
		}

		[Route("product/a")]
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
						var entity = _converterHelper.BuildProductEntity(model);
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

		[Route("product/i/{id:guid}")]
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
						var model = _converterHelper.BuildProductViewModel(entity);
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

		[Route("product/e/")]
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
						var entity = _converterHelper.BuildProductEntity(model);
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

		[Route("product/d/{id:guid}")]
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
