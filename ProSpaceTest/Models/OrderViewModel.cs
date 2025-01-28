namespace ProSpaceTest.Models
{
	public class OrderViewModel
	{
		public Guid Id { get; set; }
		public Guid CustomerId { get; set; }
		public DateTime OrderDate { get; set; }
		public DateTime ShipmentDate { get; set; }
		public int OrderNumber { get; set; }
		public string Status { get; set; }
	}
}
