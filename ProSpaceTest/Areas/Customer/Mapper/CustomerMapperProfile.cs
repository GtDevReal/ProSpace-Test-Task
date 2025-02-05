using AutoMapper;
using ProSpaceTest.Areas.Customer.Models;
using ProSpaceTest.Data.Entity;

namespace ProSpaceTest.Areas.Customer.Mapper
{
	public class CustomerMapperProfile : Profile
	{
		public CustomerMapperProfile()
		{
			CreateMap<OrderItemViewModel, OrderItemEntity>().ReverseMap();
			CreateMap<OrderViewModel, OrderEntity>().ReverseMap();
			CreateMap<ProductViewModel, ProductEntity>().ReverseMap();
		}
	}
}
