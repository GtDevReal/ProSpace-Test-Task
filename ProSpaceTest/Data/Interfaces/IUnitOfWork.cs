using ProSpaceTest.Data.Entity;

namespace ProSpaceTest.Data.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IProductsRepository Products { get; }
		Task<int> SaveChangesAsync();
	}
}
