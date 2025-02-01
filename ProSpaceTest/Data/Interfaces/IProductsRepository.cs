using Microsoft.EntityFrameworkCore;
using ProSpaceTest.Data.Entity;

namespace ProSpaceTest.Data.Interfaces
{
	public interface IProductsRepository
	{
		Task CreateAsync(ProductEntity entity);
		Task<IEnumerable<ProductEntity>> GetAllAsync();
		Task<ProductEntity> GetByIdAsync(Guid id);
		void Update(ProductEntity entity);
		void Delete(ProductEntity entity);
	}
}
