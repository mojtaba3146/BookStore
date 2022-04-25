using BookStore.Entities;
using BookStore.Infrastructure.Application;
using BookStore.Services.Books.Contracts;
using BookStore.Services.Books.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Books
{
    public class BookAppService : BookService
    {
        private readonly BookRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public BookAppService(BookRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Add(AddBookDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Description = dto.Description,
                Author = dto.Author,
                Pages = dto.Pages,
                CategoryId = dto.CategoryId,
            };

            _repository.Add(book);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var book = _repository.GetById(id);

            if (book == null)
            {
                throw new BookNotExsistException();
            }

            _repository.Delete(book);

            _unitOfWork.Commit();
        }

        public IList<GetBookDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(int id, UpdateBookDto dto)
        {
            var book = _repository.GetById(id);

            if (book == null)
            {
                throw new BookNotExsistException();
            }

            book.Title = dto.Title;
            book.Description = dto.Description;
            book.Author = dto.Author;
            book.Pages = dto.Pages;
            book.CategoryId = dto.CategoryId;

            _unitOfWork.Commit();
        }
    }
}
