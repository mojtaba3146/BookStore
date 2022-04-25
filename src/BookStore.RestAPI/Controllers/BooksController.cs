using BookStore.Services.Books.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookStore.RestAPI.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _service;

        public BooksController(BookService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddBookDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet]
        public IList<GetBookDto> GetAllBooks()
        {
            return _service.GetAll();
        }

        [HttpDelete]
        public void DeleteById(int id)
        {
            _service.Delete(id);
        }

        [HttpPut]
        public void Update(int id, UpdateBookDto dto)
        {
            _service.Update(id, dto);
        }
    }
}
