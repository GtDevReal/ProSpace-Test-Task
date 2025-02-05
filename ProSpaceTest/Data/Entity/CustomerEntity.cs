namespace ProSpaceTest.Data.Entity
{
	public class CustomerEntity
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public string? Address { get; set; }
		public float? Discount { get; set; }

		public List<OrderEntity>? Orders { get; set; }
		public List<UsersEntity>? Users { get; set; }
	}
}
