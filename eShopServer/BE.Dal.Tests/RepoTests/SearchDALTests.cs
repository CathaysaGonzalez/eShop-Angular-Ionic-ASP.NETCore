using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BE.Dal.Repos;
using BE.Dal.Repos.Interfaces;
using BE.Dal.Tests.RepoTests.Base;
using Xunit;

namespace BE.Dal.Tests.RepoTests
{
    [Collection("BE.Dal")]
    public class SearchDALTests : RepoTestsBase
    {
        private readonly IProductRepo _productRepo;
        private readonly IOrderRepo _orderRepo;
        public SearchDALTests()
        {
            _productRepo = new ProductRepo(Context);
            _orderRepo = new OrderRepo(Context);
            LoadDatabase();
        }
        public override void Dispose()
        {
            _productRepo.Dispose();
            _orderRepo.Dispose();
        }

        //Search01
        //Search03
        [Fact]
        public void ShouldFindAnItemThatMatchProductName()
        {
            var prods = _productRepo.SearchProducts(null, "NameProduct1").ToList();
            Assert.Single(prods);
        }

        [Fact]
        public void ShouldFindAnItemThatMatchProductDescription()
        {
            var prods = _productRepo.SearchProducts(null, "Description1").ToList();
            Assert.Single(prods);
        }

        //Search02
        //Search04
        [Fact]
        public void ShouldNotFindItemsThatMismatchProductName()
        {
            var prods = _productRepo.SearchProducts(null, "Product8").ToList();
            Assert.Empty(prods);
        }

        [Fact]
        public void ShouldNotFindItemsThatMismatchProductDescription()
        {
            var prods = _productRepo.SearchProducts(null, "Description8").ToList();
            Assert.Empty(prods);
        }

        //Search07
        [Fact]
        public void ShouldFindAnOrder()
        {
            Context.OrderId = 1;
            var order = _orderRepo.GetOrder(Context.OrderId);
            Assert.Equal("NameCustomer2", order.Name);
            Assert.Equal("444", order.PaymentNavigation.CardNumber);
        }

        //Search08
        [Fact]
        public void ShouldNotFindAnOrderWithInvalidOrderId()
        {
            Context.OrderId = 5;
            var order = _orderRepo.GetOrder(Context.OrderId);
            Assert.Null(order);
        }
    }
}
