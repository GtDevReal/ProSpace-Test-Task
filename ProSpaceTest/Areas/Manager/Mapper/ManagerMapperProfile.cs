using AutoMapper;
using ProSpaceTest.Areas.Manager.Models;
using ProSpaceTest.Data.Entity;

namespace ProSpaceTest.Areas.Manager.Mapper
{
	public class ManagerMapperProfile : Profile
	{
		public ManagerMapperProfile()
		{
			CreateMap<CustomerViewModel, CustomerEntity>().ReverseMap();
			CreateMap<ProductViewModel, ProductEntity>().ReverseMap();
			CreateMap<UsersViewModel, UsersEntity>().ReverseMap();
			CreateMap<OrderViewModel, OrderEntity>().ReverseMap();
		}
	}
}
