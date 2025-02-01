using ProSpaceTest.Data.Interfaces;

namespace ProSpaceTest.Data.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		private bool _disposed = false;

		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
			Products = new ProductsRepository(_context);
		}

		public IProductsRepository Products { get; set; }

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
