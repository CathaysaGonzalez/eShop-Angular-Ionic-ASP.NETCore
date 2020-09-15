using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dal.Repos.Interfaces;
using BE.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BE.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IProductRepo _repo;

        public SearchController(IProductRepo repo)
        {
            _repo = repo;
        }
    }
}