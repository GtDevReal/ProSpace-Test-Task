using ProSpaceTest.Data.Entity;

namespace ProSpaceTest.Data.Interfaces
{
	public interface IOrderRepository
	{
		Task CreateOrderAsync(OrderEntity entity);
	}
}
