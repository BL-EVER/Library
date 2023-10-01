using Common.Repository;
using LibraryCatalogService.DTOs;
using LibraryCatalogService.Models;

namespace LibraryCatalogService.Interfaces.Repository
{
    public interface ICategoryRepository : IGenericRepository<Category, CreateCategoryDTO, ReadCategoryDTO, EditCategoryDTO, EditPartialCategoryDTO, QueryParamsCategoryDTO, int>
    {
    }
}
