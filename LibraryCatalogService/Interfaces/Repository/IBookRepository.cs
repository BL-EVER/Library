using Common.Repository;
using LibraryCatalogService.DTOs;
using LibraryCatalogService.Models;

namespace LibraryCatalogService.Interfaces.Repository
{
    public interface IBookRepository : IGenericRepository<Book, CreateBookDTO, ReadBookDTO, EditBookDTO, EditPartialBookDTO, QueryParamsBookDTO, string>
    {
    }
}
