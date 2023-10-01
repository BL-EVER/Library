using AutoMapper;
using Common.Repository;
using Common.Service;
using LibraryCatalogService.DTOs;
using LibraryCatalogService.Interfaces.Repository;
using LibraryCatalogService.Interfaces.Service;
using LibraryCatalogService.Models;

namespace LibraryCatalogService.Services
{
    public class BookService : GenericService<Book, CreateBookDTO, ReadBookDTO, EditBookDTO, EditPartialBookDTO, QueryParamsBookDTO, string>, IBookService
    {
        IBookRepository _repository;
        ICacheRepository _cache;
        IMapper _mapper;
        public BookService(IBookRepository repository, ICacheRepository cache, IMapper mapper) : base(repository, cache, mapper)
        {
            _repository = repository;
            _cache = cache;
            _mapper = mapper;
        }
    }
}
