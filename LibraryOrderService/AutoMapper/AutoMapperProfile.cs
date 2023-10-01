using AutoMapper;
using Common.DTOs;
using LibraryOrderService.DTOs;
using LibraryOrderService.Models;

namespace LibraryOrderService.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsPrivate || p.GetMethod.IsVirtual;
            CreateMap<Order, ReadOrderDTO>();
            CreateMap<CreateOrderDTO, Order>();
            CreateMap<EditOrderDTO, Order>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<EditPartialOrderDTO, Order>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
