using BookStore.Entities;
using BookStore.Services.Books.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Persistence.EF.Books
{
    public class EFBookRepository : BookRepository
    {
        private readonly EFDataContext _dataContext;

        public EFBookRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void Add(Book book)
        {
            _dataContext.Books.Add(book);
        }

        public void Delete(Book book)
        {
            _dataContext.Books.Remove(book);
        }

        public IList<GetBookDto> GetAll()
        {
            return _dataContext.Books.Select(x => new GetBookDto
            {
                Title = x.Title,
                Author = x.Author,
                Pages = x.Pages,
                Description = x.Description,
                CategoryId = x.CategoryId,
            }).ToList();
        }

        public Book GetById(int id)
        {
            return _dataContext.Books
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
