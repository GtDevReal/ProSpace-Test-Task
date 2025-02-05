using Microsoft.EntityFrameworkCore;
using ProSpaceTest.Data.Entity;
using ProSpaceTest.Data.Interfaces;

namespace ProSpaceTest.Data.Repositories
{
	public class OrderRepository : IOrderRepository
	{
		private readonly ApplicationDbContext _context;
		public OrderRepository(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task CreateOrderAsync(OrderEntity entity)
		{
			await _context.Orders.AddAsync(entity);
		}

		public void DeleteOrder(OrderEntity entity)
		{
			_context.Orders.Remove(entity);
		}

		public async Task<List<OrderEntity>> GetAllOrdersAsync()
		{
			return await _context.Orders.ToListAsync();
		}

		public async Task<List<OrderEntity>> GetAllOrdersByCustomerIdAsync(Guid? id)
		{
			return await _context.Orders.Where(o => o.CustomerId == id).ToListAsync();
		}

		public async Task<OrderEntity> GetOrderByIdAsync(Guid id)
		{
			return await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
		}

		public void UpdateOrder(OrderEntity entity)
		{
			_context.Orders.Update(entity);
		}
	}
}
