﻿using BookStore.Services.Categories.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookStore.RestAPI.Controllers
{
    [Route("api/catgeories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _service;
        public CategoriesController(CategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddCategoryDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet]
        public IList<GetCategoryDto> GetAllCategory()
        {
            return _service.GetAll();
        }

        [HttpDelete]
        public void DeleteById(int id)
        {
            _service.Delete(id);
        }

        [HttpPut]
        public void Update(int id, UpdateCategoryDto dto)
        {
            _service.Update(id, dto);
        }
    }
}
