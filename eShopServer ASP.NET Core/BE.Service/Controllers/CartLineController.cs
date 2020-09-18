using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using BE.Dal.Repos.Interfaces;
using BE.Models.Entities;
using BE.Models.ViewModels;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BE.Service.Controllers 
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class CartLineController : ControllerBase
    {
        private readonly ICartLineRepo _repo;
        private readonly IOrderRepo _repoOrder;
        public CartLineController(ICartLineRepo repo, IOrderRepo repoOrder)
        {
            _repo = repo;
            _repoOrder = repoOrder;
        }

        [HttpGet("{id}", Name = "GetCartLine")]
        [Produces("application/json")]
        [ProducesResponseType(200)] //Returns single Shopping Cart Record
        [ProducesResponseType(404)] //Returned when no record was found
        [ProducesResponseType(500)] //Returned when there was an error in the repo
        public ActionResult<CartLine> GetCartLine(long id)
        {
            CartLine cartLine = _repo.Find(id);
            return cartLine ?? (ActionResult<CartLine>)NotFound();
        }

        [HttpGet("order/{id}",Name = "GetCartLinesCart")]
        [Produces("application/json")]
        [ProducesResponseType(200)] //Returns all Shopping Cart Records for the order
        [ProducesResponseType(204)] //Returned when no content in the response
        [ProducesResponseType(500)] //Returned when there was an error in the repo
        public ActionResult<List<CartLine>> GetCartLines(long id)
        {
            var cartLines = _repo.GetCartLinesByOrder(id);
            if (cartLines == null)
            {
                return NoContent();
            }
            return Ok(cartLines);
        }
        
        [HttpPost("{orderId}", Name = "AddCartLine")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)] //Bad OrderId
        [ProducesResponseType(500)] //Error in the repo
        public ActionResult AddCartLine(long orderId, CartLine record)
        {
            if (record == null || orderId != record.OrderId || !ModelState.IsValid)
            {
                return BadRequest();
            }
            record.OrderId = orderId;
            _repo.Context.OrderId = orderId;
            _repo.Add(record);
            return Ok();
        }

        [HttpDelete("{cartLineId}", Name = "DeleteCartLine")]
        [ProducesResponseType(204)] //No content
        [ProducesResponseType(404)] //Record not found
        [ProducesResponseType(500)]
        public ActionResult DeleteCartLine(long cartLineId, CartLine record)
        {
            if (cartLineId != record.Id)
            {
                return NotFound();
            }
            _repo.Context.OrderId = record.OrderId;
            _repo.Delete(record);
            var shoppingCartRecords = _repo.GetCartLinesByOrder(record.OrderId).ToList();
            _repoOrder.GetTotalPriceAndPersist(record.OrderId, shoppingCartRecords);
            return NoContent();
        }
  }
}