using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dal.Repos.Interfaces;
using BE.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo _repo;
        
        public CategoryController(ICategoryRepo repo)
        {
            _repo = repo;
        }

        [AllowAnonymous]
        [HttpGet(Name = "GetCategories")]
        public IActionResult GetCategories()
        {
            IEnumerable<Category> categories = _repo.GetCategories();
            return Ok(categories);
        }
    }
}