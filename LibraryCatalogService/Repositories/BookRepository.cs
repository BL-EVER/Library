using AutoMapper;
using LibraryCatalogService.DTOs;
using LibraryCatalogService.Interfaces.Repository;
using LibraryCatalogService.Models;
using Microsoft.EntityFrameworkCore;
using Common.Repository;
using Common.DTOs;

namespace LibraryCatalogService.Repositories
{
    public class BookRepository : GenericRepository<Book, CreateBookDTO, ReadBookDTO, EditBookDTO, EditPartialBookDTO, QueryParamsBookDTO, string>, IBookRepository
    {
        private DbContext _dbContext;
        private IMapper _mapper;

        public BookRepository(DbContext context, IMapper mapper) : base(context, mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }
    }
}
