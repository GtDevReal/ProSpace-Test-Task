using Microsoft.EntityFrameworkCore;
using ProSpaceTest.Data.Entity;
using ProSpaceTest.Data.Interfaces;

namespace ProSpaceTest.Data.Repositories
{
	public class ProductsRepository : IProductsRepository
	{
		private readonly ApplicationDbContext _context;
		public ProductsRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task CreateAsync(ProductEntity entity)
		{
			try
			{
				await _context.Products.AddAsync(entity);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<IEnumerable<ProductEntity>> GetAllAsync()
		{
			try
			{
				return await _context.Products.ToListAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<ProductEntity> GetByIdAsync(Guid id)
		{
			try
			{
				return await _context.Products.FirstOrDefaultAsync(entity => entity.Id == id);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public void Update(ProductEntity entity)
		{
			try
			{
				_context.Products.Update(entity);

			}
			catch (Exception)
			{

				throw;
			}		
		}

		public void Delete(ProductEntity entity)
		{
			try
			{
				_context.Products.Remove(entity);

			}
			catch (Exception)
			{

				throw;
			}		
		}
	}
}
