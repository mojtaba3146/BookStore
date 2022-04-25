using BookStore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Test.Tools
{
    public  class CategoryFactory
    {
        public  Category CreateCategory()
        {
            return new Category() { Title = "dummy" };
        }
    }
}
