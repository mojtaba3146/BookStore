using BookStore.Entities;
using BookStore.Infrastructure.Application;
using BookStore.Infrastructure.Test;
using BookStore.Persistence.EF;
using BookStore.Persistence.EF.Books;
using BookStore.Persistence.EF.Categories;
using BookStore.Services.Books;
using BookStore.Services.Books.Contracts;
using BookStore.Services.Books.Exceptions;
using BookStore.Services.Categories;
using BookStore.Services.Categories.Contracts;
using BookStore.Test.Tools;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Services.Test.Unit.Books
{
    public class BookServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly BookService _sut;
        private readonly BookRepository _repository;
        public BookServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFBookRepository(_dataContext);
            _sut = new BookAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_Book_Properly()
        {
            var categoryfactory = new CategoryFactory();
            var category = categoryfactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Add(category));
            AddBookDto dto = CreateAddBookDto(category);

            _sut.Add(dto);
            var expected = _dataContext.Books.Include(x => x.Category)
                .FirstOrDefault();

            expected.Title.Should().Be(dto.Title);
            expected.Pages.Should().Be(dto.Pages);
            expected.Description.Should().Be(dto.Description);
            expected.Author.Should().Be(dto.Author);
            expected.CategoryId.Should().Be(category.Id);
        }

        [Fact]
        public void GetAll_returns_all_Books()
        {
            var categoryfactory = new CategoryFactory();
            var category = categoryfactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Add(category));
            AddBookDto dto = CreateAddBookDto(category);
            _sut.Add(dto);

            var expected = _sut.GetAll();

            expected.Should().HaveCount(1);
            expected.Should().Contain(_ => _.Title == "history");
        }

        [Fact]
        public void Update_updates_Books_properly()
        {
            var categoryfactory = new CategoryFactory();
            var category = categoryfactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Add(category));
            Book bk = CreateBookWithCategory(category);
            _dataContext.Manipulate(_ => _.Books.Add(bk));


            var book = _dataContext.Books.FirstOrDefault();
            UpdateBookDto updateDto = CreateUpdateDto();

            _sut.Update(book.Id, updateDto);

            _dataContext.Books.Should()
                .Contain(_ => _.Title == updateDto.Title);
        }

        [Fact]
        public void Delete_delete_book_properly()
        {
            var categoryfactory = new CategoryFactory();
            var category = categoryfactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Add(category));
            Book bk = CreateBookWithCategory(category);
            _dataContext.Manipulate(_ => _.Books.Add(bk));

            var book = _dataContext.Books.FirstOrDefault();

            _sut.Delete(book.Id);

            _dataContext.Books.Should()
                .NotContain(_ => _.Title == "mm");
        }

        [Fact]
        public void IF_Given_Id_Does_Not_Exist_BookNotExsistException()
        {
            var categoryfactory = new CategoryFactory();
            var category = categoryfactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Add(category));
            Book bk = CreateBookWithCategory(category);
            _dataContext.Manipulate(_ => _.Books.Add(bk));
            UpdateBookDto updateDto = CreateUpdateDto();
            int BookId = 500;

            Action expected = () => _sut.Update(BookId, updateDto);

            expected.Should().Throw<BookNotExsistException>();
        }

        [Fact]
        public void IF_Selected_Id_Does_Not_Exist_BookNotExsistException()
        {
            var categoryfactory = new CategoryFactory();
            var category = categoryfactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Add(category));
            Book bk = CreateBookWithCategory(category);
            _dataContext.Manipulate(_ => _.Books.Add(bk));
            int BookId = 500;

            Action expected = () => _sut.Delete(BookId);

            expected.Should().Throw<BookNotExsistException>();
        }
        private static AddBookDto CreateAddBookDto(Entities.Category category)
        {
            return new AddBookDto
            {
                Title = "history",
                Pages = 400,
                Description = "this is abook",
                Author = "mojtaba",
                CategoryId = category.Id
            };
        }
        private static UpdateBookDto CreateUpdateDto()
        {
            return new UpdateBookDto
            {
                Title = "mm",
                Author = "mn",
                Pages = 101,
                Description = "abc",
                CategoryId = 1
            };
        }
        private static Book CreateBookWithCategory(Category category)
        {
            return new Book { Author = "aa", Title = "tt", Pages = 20, Description = "dddd", CategoryId = category.Id };
        }

    }
}
