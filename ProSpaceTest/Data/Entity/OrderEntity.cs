﻿namespace ProSpaceTest.Data.Entity
{
	public class OrderEntity
	{
		public Guid Id { get; set; }
		public Guid CustomerId { get; set; }
		public DateOnly OrderDate { get; set; }
		public DateOnly? ShipmentDate { get; set; }
		public int? OrderNumber { get; set; }
		public string? Status { get; set; }

		public CustomerEntity Customer { get; set; }
		public List<OrderItemEntity> Items { get; set; }
	}
}
