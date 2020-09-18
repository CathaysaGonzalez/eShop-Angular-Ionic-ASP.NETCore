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
    public class SupplierController : ControllerBase
    {

        private readonly ISupplierRepo _repo;

        public SupplierController(ISupplierRepo repo)
        {
            _repo = repo;
        }

        //[Authorize(Roles = "Admins")]
        [AllowAnonymous]
        [HttpGet(Name = "GetSuppliers")]
        public IActionResult GetSuppliers()
        {
            IEnumerable<Supplier> suppliers = _repo.GetAll();
            return Ok(suppliers);
        }
    }
}