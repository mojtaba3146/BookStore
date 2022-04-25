using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Services.Categories.Contracts
{
    public class AddCategoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

}
