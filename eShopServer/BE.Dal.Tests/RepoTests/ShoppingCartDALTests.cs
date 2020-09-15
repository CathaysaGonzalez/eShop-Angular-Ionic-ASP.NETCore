using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BE.Dal.Repos;
using BE.Dal.Repos.Interfaces;
using BE.Dal.Tests.RepoTests.Base;
using BE.Dal.Exceptions;
using BE.Models.Entities;
using Xunit;
using BE.Models.Entities.Base;
using System.Collections.Generic;

namespace BE.Dal.Tests.RepoTests
{
    [Collection("BE.Dal")]
    public class ShoppingCartDALTests : RepoTestsBase
    {
        private readonly ICartLineRepo _cartLineRepo;
        private readonly IOrderRepo _orderRepo;
         public ShoppingCartDALTests()
        {
            _cartLineRepo = new CartLineRepo(Context, new ProductRepo(Context));
            _orderRepo = new OrderRepo(Context);
            LoadDatabase();
        }
        public override void Dispose()
        {
            _cartLineRepo.Dispose();
            _orderRepo.Dispose();
        }

        //ShoppingCart02
        [Fact]
        public void ShouldRetrieveAllItemsFromShoppingCart()
        {
            Context.OrderId = 1;
            var shoppingCartRecords = _cartLineRepo.GetCartLinesByOrder(Context.OrderId).ToList();
            Assert.Equal(2, shoppingCartRecords.Count);
            Assert.Equal(2, shoppingCartRecords[0].ProductId);
            Assert.Equal(4, shoppingCartRecords[0].Quantity);
            Assert.Equal(1, shoppingCartRecords[1].ProductId);
            Assert.Equal(3, shoppingCartRecords[1].Quantity);
        }

        //ShoppingCart03
        [Fact]
        public void ShouldAddAnItemToShoppingCart()
        {
            Context.OrderId = 1;
            var item = new CartLine()
            {
                ProductId = 3,
                Quantity = 1,
                OrderId = 1
            };
            _cartLineRepo.Add(item);
            var shoppingCartRecords = _cartLineRepo.GetCartLinesByOrder(Context.OrderId).ToList();
            Assert.Equal(3, shoppingCartRecords.Count);
            Assert.Equal(2, shoppingCartRecords[0].ProductId);
            Assert.Equal(4, shoppingCartRecords[0].Quantity);
            Assert.Equal(1, shoppingCartRecords[1].ProductId);
            Assert.Equal(3, shoppingCartRecords[1].Quantity);
            Assert.Equal(3, shoppingCartRecords[2].ProductId);
            Assert.Equal(1, shoppingCartRecords[2].Quantity);
        }

        //ShoppingCart04
        [Fact]
        public void ShouldCalculateTotalOfANewOrder()
        {
            var order = new Order
            {
                Name = "NameCustomer1",
                Address = "AddressCustomer1",
                Shipped = false,
                CartLines = new List<CartLine>
                {
                    new CartLine
                    {
                        ProductId = 1,
                        Quantity = 1,
                    },
                    new CartLine
                    {
                        ProductId = 2,
                        Quantity = 1,
                    }
                },
                PaymentNavigation = new Payment()
                {
                    CardNumber = "111",
                    CardExpiry = "222",
                    CardSecurityCode = "333"
                }
            };
            var shoppingCartRecords = _cartLineRepo.GetCartLinesByOrder(Context.OrderId).ToList();
            Order orderResult = _orderRepo.Purchase(order);
            Assert.Equal(323.95M, orderResult.Total);
            Assert.Equal(3, orderResult.Id);
        }

        [Fact]
        public void ShouldCalculateTotalOfAnExitingOrder()
        {
            Context.OrderId = 1;
            var orders = _cartLineRepo.Context.CartLines.Where(x => x.Id == Context.OrderId);
            var total = _orderRepo.GetTotalPrice(orders);
            Assert.Equal(195.80M, total);
        }

        //ShoppingCart04
        [Fact]
        public void ShouldCalculateLineTotalOfANewProductAdded()
        {
            var item = new CartLine()
            {
                ProductId = 3,
                Quantity = 1,
                OrderId = 1
            };
            _cartLineRepo.Add(item);
            var shoppingCartRecords = _cartLineRepo.GetAll().ToList();
            Assert.Equal(19.50M, shoppingCartRecords[4].LineItemTotal);
        }

        //ShoppingCart05
        [Fact]
        public void ShouldDeleteCartRecordAndUpdateTotalNumberOfItems()
        {
            var item = _cartLineRepo.Find(1);
            _cartLineRepo.Context.Entry(item).State = EntityState.Detached;
            _cartLineRepo.Delete(item);
            Context.OrderId = 1;
            var shoppingCartRecords = _cartLineRepo.GetCartLinesByOrder(Context.OrderId).ToList();
            _orderRepo.GetTotalPriceAndPersist(Context.OrderId, shoppingCartRecords);
            var cartLines = _cartLineRepo.GetAll(x => x.Id).ToList();
            Assert.Equal(2, cartLines[0].Id);
            var order = _orderRepo.GetOrder(Context.OrderId);
            Assert.Equal(825M, order.Total);
        }

        //ShoppingCart06
        [Fact]
        public void ShouldCalculateLineTotalOfAnExistingProductWhenQuantityIncreases()
        {
            var item = new CartLine()
            {
                Id = 2,
                ProductId = 1,
                Quantity = 1,
                OrderId = 1
            };
            _cartLineRepo.Add(item);
            var shoppingCartRecords = _cartLineRepo.GetAll().ToList();
            Assert.Equal(4, shoppingCartRecords[1].Quantity);
            Assert.Equal(1100M, shoppingCartRecords[1].LineItemTotal);
        }

        //ShoppingCart07
        [Fact]
        public void ShouldCalculateLineTotalOfAnExistingProductWhenQuantityDecreases()
        {
            var item = new CartLine()
            {
                Id = 1,
                ProductId = 2,
                Quantity = -1,
                OrderId = 1
            };
            _cartLineRepo.Add(item);
            var shoppingCartRecords = _cartLineRepo.GetAll().ToList();
            Assert.Equal(3, shoppingCartRecords[0].Quantity);
            Assert.Equal(146.85M, shoppingCartRecords[0].LineItemTotal);
        }

        //ShoppingCart08
        [Fact]
        public void ShouldThrowAnErrorWhenAddingTooMuchQuantity()
        {
            var item = new CartLine()
            {
                Id = 1,
                ProductId = 2,
                Quantity = 11,
                OrderId = 1
            };
            var ex = Assert.Throws<BEInvalidQuantityException>(() => _cartLineRepo.Update(item));
            Assert.Equal("Can't add more product than available in stock", ex.Message);
        }

        //ShoppingCart09
        [Fact]
        public void ShouldDeleteAnItemOnAddIfQuantityLessThanZero()
        {
            Context.OrderId = 1;
            var item = new CartLine()
            {
                Id = 2,
                ProductId = 1,
                Quantity = -11,
                OrderId = 1
            };
            _cartLineRepo.Add(item);
            var shoppingCartRecords = _cartLineRepo.GetCartLinesByOrder(Context.OrderId).ToList();
            Assert.Equal(0, shoppingCartRecords.Count(x => x.ProductId == 1));
            Assert.Single(shoppingCartRecords);
        }

        //ShoppingCart09
        [Fact]
        public void ShouldDeleteAnItemOnAddIfQuantityEqualZero()
        {
            Context.OrderId = 1;
            var item = new CartLine()
            {
                Id = 2,
                ProductId = 1,
                Quantity = -3,
                OrderId = 1
            };
            _cartLineRepo.Add(item);
            var shoppingCartRecords = _cartLineRepo.GetCartLinesByOrder(Context.OrderId).ToList();
            Assert.Equal(0, shoppingCartRecords.Count(x => x.ProductId == 1));
            Assert.Single(shoppingCartRecords);
        }

        //ShoppingCart09
        [Fact]
        public void ShouldDeleteAnItemOnUpdateIfQuantityLessThanZero()
        {
            var item = _cartLineRepo.Find(1);
            item.Quantity = -20;
            _cartLineRepo.Update(item);
            var shoppingCartRecords = _cartLineRepo.GetAll().ToList();
            Assert.Equal(3, shoppingCartRecords.Count());
        }

        //ShoppingCart09
        [Fact]
        public void ShouldDeleteAnItemOnUpdateIfQuantityEqualZero()
        {
            var item = _cartLineRepo.Find(2);
            item.Quantity = -3;
            _cartLineRepo.Update(item);
            var shoppingCartRecords = _cartLineRepo.GetAll().ToList();
            Assert.Equal(3, shoppingCartRecords.Count());
        }
    }
}