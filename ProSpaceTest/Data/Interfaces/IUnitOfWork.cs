using ProSpaceTest.Data.Entity;

namespace ProSpaceTest.Data.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IOrderRepository Orders { get; }
		IProductsRepository Products { get; }
		ICustomersRepository Customers { get; }
		IUsersRepository AspNetUsers { get; }
		Task<int> SaveChangesAsync();
	}
}
