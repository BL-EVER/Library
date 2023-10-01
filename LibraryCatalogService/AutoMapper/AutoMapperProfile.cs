using AutoMapper;
using LibraryCatalogService.DTOs;
using LibraryCatalogService.Models;

namespace LibraryCatalogService.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Book, ReadBookDTO>().ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
            CreateMap<Book, ReadPartialBookDTO>();
            CreateMap<CreateBookDTO, Book>();
            CreateMap<EditBookDTO, Book>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<EditPartialBookDTO, Book>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Category, ReadCategoryDTO>().ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));
            CreateMap<Category, ReadPartialCategoryDTO>();
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<EditCategoryDTO, Category>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<EditPartialCategoryDTO, Category>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
