using Common.Service;
using LibraryCatalogService.DTOs;
using LibraryCatalogService.Models;

namespace LibraryCatalogService.Interfaces.Service
{
    public interface ICategoryService : IGenericService<Category, CreateCategoryDTO, ReadCategoryDTO, EditCategoryDTO, EditPartialCategoryDTO, QueryParamsCategoryDTO, int>
    {
    }
}
