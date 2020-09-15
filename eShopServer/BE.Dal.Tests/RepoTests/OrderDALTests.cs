using System;
using System.Collections.Generic;
using System.Text;
using BE.Dal.Repos;
using BE.Dal.Repos.Interfaces;
using BE.Dal.Tests.RepoTests.Base;
using BE.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BE.Dal.Tests.RepoTests
{
    [Collection("BE.Dal")]
    public class OrderDALTests : RepoTestsBase
    {
        private readonly IOrderRepo _orderRepo;

        public OrderDALTests()
        {
            _orderRepo = new OrderRepo(Context);
            LoadDatabase();
        }

        public override void Dispose()
        {
            _orderRepo.Dispose();
        }

        //AdminAccount10
        [Fact]
        public void ShouldUpdateOrderDetails()
        {
            var order = new Order
            {
                Name = "NewName",
                Address = "NewAddress",
                Shipped = true,
            };
            _orderRepo.AddRange(new List<Order>
                {
                    order
                });
            order.Name = "NewNewName";
            _orderRepo.Update(order, false);
            var item = _orderRepo.Find(order.Id);
            var count = _orderRepo.SaveChanges();
            Assert.Equal(1, count);
            Assert.Equal("NewNewName", item.Name);
            Assert.Equal("NewAddress", item.Address);
        }

        //AdminAccount12
        [Fact]
        public void ShouldDeleteAnOrder()
        {
            var item = _orderRepo.Find(1);
            _orderRepo.Context.Entry(item).State = EntityState.Detached;
            _orderRepo.Delete(item);
            Context.OrderId = 1;
            var order = _orderRepo.Find(Context.OrderId);
            Assert.Null(order);
        }

        //AdminAccount13
        [Fact]
        public void ShouldRetrieveDetailsOfAnOrder()
        {
            Context.OrderId = 1;
            var order = _orderRepo.Find(Context.OrderId);
            Assert.Equal("NameCustomer2", order.Name);
            Assert.Equal("AddressCustomer2", order.Address);
            Assert.False(order.Shipped);
        }
    }
}
