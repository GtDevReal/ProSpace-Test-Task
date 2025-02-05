using Microsoft.AspNetCore.Identity;
using ProSpaceTest.Data.Entity;
using ProSpaceTest.Data.Interfaces;

namespace ProSpaceTest.Data.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<UsersEntity> _userManager;
		private bool _disposed = false;

		public IProductsRepository Products { get; set; }
		public ICustomersRepository Customers { get; set; }
		public IUsersRepository AspNetUsers { get; set; }
		public IOrderRepository Orders { get; set; }

		public UnitOfWork(ApplicationDbContext context, UserManager<UsersEntity> userManager)
		{
			_context = context;
			_userManager = userManager;

			Orders = new OrderRepository(_context);
			Products = new ProductsRepository(_context);
			Customers = new CustomersRepository(_context);
			AspNetUsers = new UsersRepository(_context, _userManager);
		}

		public async Task<int> SaveChangesAsync()
		{
			return await _context.SaveChangesAsync();
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_context.Dispose();
				}
				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
