using Microsoft.EntityFrameworkCore;
using ProSpaceTest.Data.Entity;
using ProSpaceTest.Data.Interfaces;

namespace ProSpaceTest.Data.Repositories
{
	public class CustomersRepository : ICustomersRepository
	{
		private readonly ApplicationDbContext _context;
		public CustomersRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<CustomerEntity>> GetAllCustomersAsync()
		{
			try
			{
				return await _context.Customers.ToListAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task CreateCustomer(CustomerEntity entity)
		{
			try
			{
				await _context.Customers.AddAsync(entity);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public async Task<CustomerEntity> GetCustomerByIdAsync(Guid id)
		{
			try
			{
				return await _context.Customers.FirstOrDefaultAsync(e => e.Id == id);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public void DeleteCustomer(CustomerEntity entity)
		{
			try
			{
				_context.Remove(entity);
			}
			catch (Exception)
			{

				throw;
			}
		}

		public void UpdateCustomer(CustomerEntity entity)
		{
			try
			{
				_context.Customers.Update(entity);
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
