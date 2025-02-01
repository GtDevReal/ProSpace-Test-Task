using ProSpaceTest.Areas.Manager.Models;
using ProSpaceTest.Data.Entity;
using ProSpaceTest.Models;

namespace ProSpaceTest.Infrastructure
{
	public class ConverterHelper : IConverterHelper
	{
		public CustomerViewModel BuildCutomerViewModel(CustomerEntity entity)
		{
			CustomerViewModel model = new()
			{ 
				Id = entity.Id,
				Name = entity.Name,
				Address = entity.Address,
				Code = entity.Code,
				Discount = entity.Discount,
			};

			return model;
		}

		public OrderItemViewModel BuildOrderItemViewModel(OrderItemEntity entity)
		{
			OrderItemViewModel model = new()
			{
				Id = entity.Id,
				ItemId = entity.ItemId,
				ItemPrice = entity.ItemPrice,
				ItemsCount = entity.ItemsCount,
				OrderId = entity.OrderId
			};
			return model;
		}

		public OrderViewModel BuildOrderViewModel(OrderEntity entity)
		{
			OrderViewModel model = new()
			{
				Id = entity.Id,
				CustomerId = entity.CustomerId,
				OrderDate = entity.OrderDate,
				ShipmentDate = entity.ShipmentDate,
				OrderNumber = entity.OrderNumber,
				Status = entity.Status,
			};
			return model;
		}

		public ProductViewModel BuildProductViewModel(ProductEntity entity)
		{
			ProductViewModel model = new()
			{
				Id = entity.Id,
				Category = entity.Category,
				Code = entity.Code,
				Name = entity.Name,
				Price = entity.Price,
			};

			return model;
		}

		#region To entity

		public CustomerEntity BuildCustomerEntity(CustomerViewModel model)
		{
			CustomerEntity entity = new()
			{
				Id = model.Id,
				Name = model.Name,
				Code = model.Code,
				Address = model.Address,
				Discount = model.Discount,
			};

			return entity;
		}
		public OrderItemEntity BuildOrderItemEntity(OrderItemViewModel model)
		{
			OrderItemEntity entity = new()
			{
				Id = model.Id,
				ItemId = model.ItemId,
				ItemPrice = model.ItemPrice,
				ItemsCount = model.ItemsCount,
				OrderId = model.OrderId,
			};
			return entity;
		}
		public OrderEntity BuildOrderEntity(OrderViewModel model)
		{
			OrderEntity entity = new()
			{
				OrderDate = model.OrderDate,
				CustomerId = model.CustomerId,
				Id = model.Id,
				OrderNumber = model.OrderNumber,
				ShipmentDate = model.ShipmentDate,
				Status = model.Status,
			};
			return entity;
		}
		public ProductEntity BuildProductEntity(ProductViewModel model)
		{
			ProductEntity entity = new()
			{
				Id = model.Id,
				Category = model.Category,
				Code = model.Code,
				Name = model.Name,
				Price = model.Price,
			};
			return entity;
		}

		#endregion
	}
}
