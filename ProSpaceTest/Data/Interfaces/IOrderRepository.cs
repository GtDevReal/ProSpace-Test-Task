using ProSpaceTest.Data.Entity;

namespace ProSpaceTest.Data.Interfaces
{
	public interface IOrderRepository
	{
		Task CreateOrderAsync(OrderEntity entity);
		Task<List<OrderEntity>> GetAllOrdersByCustomerIdAsync(Guid? id);
		Task<OrderEntity> GetOrderByIdAsync(Guid id);
		void DeleteOrder(OrderEntity entity);
		Task<List<OrderEntity>> GetAllOrdersAsync();

		void UpdateOrder(OrderEntity entity);
	}
}
