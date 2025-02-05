
namespace ProSpaceTest.Areas.Customer.Models
{
	public class OrderItemViewModel
	{
		public Guid Id { get; set; }
		public Guid OrderId { get; set; }
		public Guid ItemId { get; set; }
		public int ItemsCount { get; set; }
		public float ItemPrice { get; set; }

		public OrderViewModel Order { get; set; }
	}
}
