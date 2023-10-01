using Common.Service;
using LibraryCatalogService.DTOs;
using LibraryCatalogService.Models;

namespace LibraryCatalogService.Interfaces.Service
{
    public interface IBookService : IGenericService<Book, CreateBookDTO, ReadBookDTO, EditBookDTO, EditPartialBookDTO, QueryParamsBookDTO, string>
    {
    }
}
