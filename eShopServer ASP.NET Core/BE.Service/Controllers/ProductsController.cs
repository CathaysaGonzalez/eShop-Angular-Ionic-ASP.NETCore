using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BE.Dal.Repos.Interfaces;
using BE.Models.Entities;
using BE.Models.Entities.Base;
using BE.Dal.EfStructures;
using BE.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BE.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepo _repo;

        public ProductsController(IProductRepo repo, BEIdentityContext ctx)
        {
            _repo = repo;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetProductsWithNavigationProperties()
        {
            IEnumerable<Product> products = _repo.GetProductsWithNavigationProperties();
            if (products != null)
            {
                if (products.Count() == 0)
                    return NoContent();

                return Ok(products);
            }
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpGet("{id}", Name = "GetProduct")]
        [Produces("application/json")]
        [ProducesResponseType(200)] //Returns single Product
        [ProducesResponseType(404)] //Returned when Product with specific id doesn't exist
        [ProducesResponseType(500)] //Returned when there was an error in the repo
        public ActionResult<Product> GetProduct(long id)
        {
            Product item = _repo.GetProductWithNavigationProperties(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [AllowAnonymous]
        [HttpGet("search", Name = "SearchProducts")]
        [Produces("application/json")]
        [ProducesResponseType(200)] //Returns matching products
        [ProducesResponseType(204)] //Returned when no content in the response
        [ProducesResponseType(500)] //Returned when there was an error in the repo
        public IActionResult SearchProducts(string category, string search, bool related = false)
        {
            IQueryable<Product> query = _repo.SearchProducts(category, search);
            if (query != null)
            {
                if (query.Count() == 0)
                    return NoContent();
                if (related)
                {
                    List<Product> data = _repo.SearchProductsWithNavigationProperties(category, search, query);
                    return Ok(data);
                }
                else
                {
                    return Ok(query);
                }
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admins")]
        [HttpPost(Name = "CreateProduct")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult CreateProduct([FromBody] Product pdata)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(pdata);
                return Ok(pdata.Id);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = "Admins")]
        [HttpPut("{id}", Name = "ReplaceProduct")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult ReplaceProduct(long id, [FromBody] Product pdata)
        {
            if (ModelState.IsValid)
            {
                pdata.Id = id;

                _repo.Update(pdata);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = "Admins")]
        [HttpDelete("{id}", Name = "DeleteProduct")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public void DeleteProduct(long id)
        {
            _repo.Delete(new Product { Id = id });
        }

    }
}
