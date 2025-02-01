using Microsoft.EntityFrameworkCore;
using ProSpaceTest.Data.Entity;
using ProSpaceTest.Data.Interfaces;
using SQLitePCL;

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
			await _context.Products.AddAsync(entity);
		}

		public async Task<IEnumerable<ProductEntity>> GetAllAsync()
		{
			return await _context.Products.ToListAsync();
		}

		public async Task<ProductEntity> GetByIdAsync(Guid id)
		{
			return await _context.Products.FirstOrDefaultAsync(entity => entity.Id == id);
		}

		public void Update(ProductEntity entity)
		{
			_context.Products.Update(entity);
		}

		public void Delete(ProductEntity entity)
		{
			_context.Products.Remove(entity);
		}
	}
}
