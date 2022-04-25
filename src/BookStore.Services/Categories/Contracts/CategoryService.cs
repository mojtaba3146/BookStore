using BookStore.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Categories.Contracts
{
    public interface CategoryService : Service
    {
        void Add(AddCategoryDto dto);
        void Update(int id,UpdateCategoryDto dto);
        void Delete(int id);
        IList<GetCategoryDto> GetAll();
    }
}
