namespace ProSpaceTest.Areas.Customer.Models
{
	public class OrderViewModel
	{
		public Guid Id { get; set; }
		public Guid CustomerId { get; set; }
		public DateOnly OrderDate { get; set; }
		public DateOnly? ShipmentDate { get; set; }
		public int? OrderNumber { get; set; }
		public string? Status { get; set; }

		public List<OrderItemViewModel> Items { get; set; }
	}
}
