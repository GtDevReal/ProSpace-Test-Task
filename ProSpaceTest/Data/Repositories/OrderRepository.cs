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
	}
}
