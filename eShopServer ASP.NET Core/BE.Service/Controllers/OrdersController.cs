using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BE.Dal.EfStructures;
using BE.Dal.Repos.Interfaces;
using BE.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BE.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepo _repo;
        public OrdersController(IOrderRepo repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "Admins")]
        [HttpGet(Name = "GetOrdersHistory")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetOrdersHistory()
        {
            IEnumerable<Order> orders = _repo.GetOrders();
            return orders == null ? (IActionResult)NotFound() : new ObjectResult(orders);
        }

        [Authorize(Roles = "Admins")]
        [HttpGet("{id}", Name = "GetOrder")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Order> GetOrder(long id)
        {
            if (ModelState.IsValid)
            {
                var item = _repo.GetOrder(id);
                if (item == null)
                {
                    return NotFound();
                }

                return Ok(item);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = "Users")]
        [HttpGet("user", Name = "GetOrdersForCurrentCustomer")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Order> GetOrdersForCurrentCustomer()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            IEnumerable<Order> orders = _repo.GetOrdersByUserId(userId);
            return orders == null ? (ActionResult)NotFound() : new ObjectResult(orders);
        }

        [Authorize(Roles = "Admins")]
        [HttpGet("orders/cartlines", Name = "GetOrdersWithPaymentAndCartLines")]
        public IActionResult GetOrdersWithCartLines()
        {
            IEnumerable<Order> orders = _repo.GetOrdersWithNavigationProperties();
            if (orders != null)
            {
                if (orders.Count() == 0)
                    return NoContent();

                return Ok(orders);
            }

            return BadRequest();
        }

        [Authorize(Roles = "Admins")]
        [HttpGet("details/{id}", Name = "GetOrderWithDetails")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Order> GetOrderWithDetails(long id)
        {
            if (ModelState.IsValid)
            {
                var item = _repo.GetOrderWithNavigationProperties(id);
                if (item == null)
                {
                    return NotFound();
                }

                return Ok(item);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = "Users")]
        [HttpGet("username/{id}", Name = "GetOrdersForACustomerName")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public ActionResult<Order> GetOrdersForACustomerName(string id)
        {
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            IEnumerable<Order> orders = _repo.GetOrdersByUserName(id);
            return orders == null ? (ActionResult)NotFound() : new ObjectResult(orders);
        }

        [Authorize(Roles = "Users")]
        [HttpPost(Name = "CreateOrder")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateOrder([FromBody] Order order)
        {
            if (ModelState.IsValid)
            {
                //order.UserName = User.Identity.Name;
                order.UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _repo.Purchase(order);
                return Ok(new
                {
                    user = order.UserId,
                    orderId = order.Id,
                    authCode = order.PaymentNavigation.AuthCode,
                    amount = order.Total
                });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = "Admins")]
        [HttpPost("shipped/{id}", Name = "MarkAsShipped")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult MarkAsShipped(long id)
        {
            if (ModelState.IsValid)
            {
                _repo.MarkShipped(id);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = "Admins")]
        [HttpPut("{id}", Name = "ModifyOrder")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult ReplaceOrder(long id, [FromBody] Order order)
        {
            if (ModelState.IsValid)
            {
                order.Id = id;
                _repo.Update(order);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = "Users, Admins")]
        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(204)] //Returned when a order was deleted
        [ProducesResponseType(500)] //Returned when there was an error in the repo
        public IActionResult DeleteOrder(long id)
        {
            _repo.Delete(new Order { Id = id });
            return NoContent();
        }



        [AllowAnonymous]
        [HttpGet("test/{id}", Name = "GetOrderForTest")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Order> GetOrderForTest(long id)
        {
            var item = _repo.GetOrderWithCartLines(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [AllowAnonymous]
        [HttpGet("test", Name = "GetOrdersHistoryForTest")]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult GetOrdersHistoryForTest()
        {
            IEnumerable<Order> orders = _repo.GetOrders();
            return orders == null ? (IActionResult)NotFound() : new ObjectResult(orders);
        }
    }
}