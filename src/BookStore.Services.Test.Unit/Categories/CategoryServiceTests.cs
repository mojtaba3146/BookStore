using BookStore.Entities;
using BookStore.Infrastructure.Application;
using BookStore.Infrastructure.Test;
using BookStore.Persistence.EF;
using BookStore.Persistence.EF.Categories;
using BookStore.Services.Categories;
using BookStore.Services.Categories.Contracts;
using BookStore.Services.Categories.Exceptions;
using BookStore.Test.Tools;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookStore.Services.Test.Unit.Categories
{
    public class CategoryServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;
        
        public CategoryServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository,_unitOfWork);
        }

        [Fact]
        public void Add_adds_category_properly()
        {
            AddCategoryDto dto = GenerateAddCategoryDto();

            _sut.Add(dto);

            _dataContext.Categories.Should()
                .Contain(_ => _.Title == dto.Title);
        }

        [Fact]
        public void GetAll_returns_all_categories()
        {
            CreateCategoriesInDataBase();

            var expected = _sut.GetAll();

            expected.Should().HaveCount(3);
            expected.Should().Contain(_ => _.Title == "dummy1");
            expected.Should().Contain(_ => _.Title == "dummy2");
            expected.Should().Contain(_ => _.Title == "dummy3");
        }

        [Fact]
        public void Update_updates_category_properly()
        {
            var categoryfactory = new CategoryFactory();
            var category = categoryfactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Add(category));
            var dto = CreateUpdateCategoryDto("moji");

            _sut.Update(category.Id,dto);

            _dataContext.Categories.Should()
                .Contain(_ => _.Title==dto.Title);
        }

        [Fact]
        public void Delete_delete_category_properly()
        {
            var categoryfactory = new CategoryFactory();
            var category = categoryfactory.CreateCategory();
            _dataContext.Manipulate(_ => _.Add(category));

            _sut.Delete(category.Id);

            _dataContext.Categories.Should()
                .NotContain(_ => _.Title == "dummy");
        }

        [Fact]
        public void IF_Given_Id_Does_Not_Exist_For_Update_Exception()
        {
            int categoryId = 500;

            Action expected = () => _sut.Update(categoryId, CreateUpdateCategoryDto("mm"));

            expected.Should().Throw<GroupNotExsistException>();
        }

        [Fact]
        public void IF_Selected_Id_Does_Not_Exist_For_Delete_Exception()
        {
            CreateCategoriesInDataBase();
            int categoryId = 500;

            Action expected = () => _sut.Delete(categoryId);

            expected.Should().Throw<GroupNotExsistException>();
        }

        private static UpdateCategoryDto CreateUpdateCategoryDto(string title)
        {
            return new UpdateCategoryDto { Title = title };
        }

        private void CreateCategoriesInDataBase()
        {
            var categories = new List<Category>
            {
                new Category { Title = "dummy1"},
                new Category { Title = "dummy2"},
                new Category { Title = "dummy3"}
            };
            _dataContext.Manipulate(_ =>
            _.Categories.AddRange(categories));
        }

        private static AddCategoryDto GenerateAddCategoryDto()
        {
            return new AddCategoryDto
            {
                Title = "dummy"
            };
        }
    }
}
