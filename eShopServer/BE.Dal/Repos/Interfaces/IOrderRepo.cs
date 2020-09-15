using System;
using System.Collections.Generic;
using BE.Dal.Repos.Base;
using BE.Models.Entities;


namespace BE.Dal.Repos.Interfaces
{
    public interface IOrderRepo : IRepo<Order>
    {
        Order GetOrderWithNavigationProperties(long orderId);
        Order Purchase(Order order);
        decimal GetTotalPrice(IEnumerable<CartLine> lines);
        IEnumerable<Order> GetOrdersByUserName(string userName);
        int Update(Order entity, bool persist = true);
        Order GetOrder(long orderId);
        decimal GetTotalPriceAndPersist(long orderId, List<CartLine> cartLines);
        IEnumerable<Order> GetOrders();
        public Order GetOrderWithCartLines(long orderId);
        IEnumerable<Order> GetOrdersWithNavigationProperties();
        void MarkShipped(long id);
        IEnumerable<Order> GetOrdersByUserId(string userId);
    }
}