using AutoMapper;
using Common.DTOs;
using Common.Repository;
using LibraryCatalogService.DTOs;
using LibraryCatalogService.Interfaces.Repository;
using LibraryCatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryCatalogService.Repositories
{
    public class CategoryRepository : GenericRepository<Category, CreateCategoryDTO, ReadCategoryDTO, EditCategoryDTO, EditPartialCategoryDTO, QueryParamsCategoryDTO, int>, ICategoryRepository
    {
        private DbContext _dbContext;
        private IMapper _mapper;

        public CategoryRepository(DbContext context, IMapper mapper) : base(context, mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }
    }
}
