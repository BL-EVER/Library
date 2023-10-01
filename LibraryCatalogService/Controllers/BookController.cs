using Common.Controller;
using LibraryCatalogService.DTOs;
using LibraryCatalogService.Interfaces.Service;
using LibraryCatalogService.Models;

namespace LibraryCatalogService.Controllers
{
    public class BookController : GenericController<Book, CreateBookDTO, ReadBookDTO, EditBookDTO, EditPartialBookDTO, QueryParamsBookDTO, string>
    {
        IBookService _service;
        public BookController(IBookService service) : base(service)
        {
            _service = service;
        }
    }
}
