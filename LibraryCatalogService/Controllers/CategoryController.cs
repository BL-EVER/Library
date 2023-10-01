using Common.Controller;
using LibraryCatalogService.DTOs;
using LibraryCatalogService.Interfaces.Service;
using LibraryCatalogService.Models;

namespace LibraryCatalogService.Controllers
{
    public class CategoryController : GenericController<Category, CreateCategoryDTO, ReadCategoryDTO, EditCategoryDTO, EditPartialCategoryDTO, QueryParamsCategoryDTO, int>
    {
        ICategoryService _service;

        public CategoryController(ICategoryService service) : base(service)
        {
            _service = service;
        }
    }
}
