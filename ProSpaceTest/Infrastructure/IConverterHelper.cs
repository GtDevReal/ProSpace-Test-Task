using ProSpaceTest.Areas.Manager.Models;
using ProSpaceTest.Data.Entity;
using ProSpaceTest.Models;

namespace ProSpaceTest.Infrastructure
{
	public interface IConverterHelper
	{
		CustomerViewModel BuildCutomerViewModel(CustomerEntity entity);
		OrderItemViewModel BuildOrderItemViewModel(OrderItemEntity entity);
		OrderViewModel BuildOrderViewModel(OrderEntity entity);
		ProductViewModel BuildProductViewModel(ProductEntity entity);

		#region To entity

		CustomerEntity BuildCustomerEntity(CustomerViewModel model);
		OrderItemEntity BuildOrderItemEntity(OrderItemViewModel model);
		OrderEntity BuildOrderEntity(OrderViewModel model);
		ProductEntity BuildProductEntity(ProductViewModel model);

		#endregion
	}
}
