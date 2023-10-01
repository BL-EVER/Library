using AutoMapper;
using Common.Repository;
using Common.Service;
using LibraryCatalogService.DTOs;
using LibraryCatalogService.Interfaces.Repository;
using LibraryCatalogService.Interfaces.Service;
using LibraryCatalogService.Models;

namespace LibraryCatalogService.Services
{
    public class CategoryService : GenericService<Category, CreateCategoryDTO, ReadCategoryDTO, EditCategoryDTO, EditPartialCategoryDTO, QueryParamsCategoryDTO, int>, ICategoryService
    {
        ICategoryRepository _repository;
        ICacheRepository _cache;
        IMapper _mapper;

        public CategoryService(ICategoryRepository repository, ICacheRepository cache, IMapper mapper) : base(repository, cache, mapper)
        {
            _repository = repository;
            _cache = cache;
            _mapper = mapper;
        }
    }
}
