namespace ProSpaceTest.Areas.Manager.Models
{
	public class CustomerViewModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public string Address { get; set; }
		public float Discount { get; set; }

		public List<UsersViewModel> Users { get; set; }
	}
}
