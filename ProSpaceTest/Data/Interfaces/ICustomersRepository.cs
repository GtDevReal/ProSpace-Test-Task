using ProSpaceTest.Data.Entity;

namespace ProSpaceTest.Data.Interfaces
{
	public interface ICustomersRepository
	{
		Task<List<CustomerEntity>> GetAllCustomersAsync();
		Task CreateCustomer(CustomerEntity entity);
		Task<CustomerEntity> GetCustomerByIdAsync(Guid id);
		void DeleteCustomer(CustomerEntity entity);
		void UpdateCustomer(CustomerEntity entity);
	}
}
