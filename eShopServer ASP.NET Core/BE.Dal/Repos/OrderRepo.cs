using BE.Dal.EfStructures;
using BE.Dal.Repos.Base;
using BE.Dal.Repos.Interfaces;
using BE.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.Configuration.CommandLine;
using Microsoft.AspNetCore.Identity;


namespace BE.Dal.Repos
{
    public class OrderRepo: RepoBase<Order>, IOrderRepo
    {
        public OrderRepo(BEIdentityContext context) : base(context)
        {
        }
        internal OrderRepo(DbContextOptions<BEIdentityContext> options) : base(options)
        {
        }
        public override void Dispose()
        { 
            base.Dispose();
        }

        public Order GetOrderWithNavigationProperties(long orderId)
        {
            Order result = Table
                .Include(o => o.PaymentNavigation)
                .Include(o => o.CartLines)
                .ThenInclude(c => c.ProductNavigation)
                .ThenInclude(p => p.Supplier)
                .FirstOrDefault(o => o.Id == orderId);
            if (result != null)
            {
                if (result.CartLines != null)
                {
                    foreach (CartLine r in result.CartLines)
                    {
                        if (r.ProductNavigation != null)
                        {
                            r.ProductNavigation.Ratings = null;
                            r.ProductNavigation.CartLines = null;
                            if (r.ProductNavigation.Supplier != null)
                            {
                                r.ProductNavigation.Supplier.Products = null;
                            }
                        }
                        r.OrderNavigation = null;
                    }
                }
                if (result.PaymentNavigation != null)
                {
                    result.PaymentNavigation.OrderNavigation = null;
                }
            }
            return result;
        }

        public override int Update(Order entity, bool persist = true)
        {
            var dbRecord = Find(entity.Id);
            dbRecord.Name = entity.Name;
            dbRecord.Address = entity.Address;
            dbRecord.Shipped = entity.Shipped;
            dbRecord.Total = entity.Total;
            dbRecord.UserId = entity.UserId;
            return base.Update(dbRecord, persist);
        }

        public Order Purchase(Order order)
        {
            order.Shipped = false;
            order.Total = GetTotalPrice(order.CartLines);
            order.PaymentNavigation.AuthCode = "1234";
            Add(order);
            return order;
        }

        public decimal GetTotalPrice(IEnumerable<CartLine> lines)
        {
            IEnumerable<long> ids = lines.Select(l => l.ProductId);
            IEnumerable<Product> prods = Context.Products.Where(p => ids.Contains(p.Id));
            return prods.Select(p => lines
                    .First(l => l.ProductId == p.Id).Quantity * p.Price)
                .Sum();
        }

        public IEnumerable<Order> GetOrdersByUserName(string userName)
        {
            IQueryable<Order> query = Context.Orders.Where(o => o.UserName == userName);
            List<Order> data = query.ToList();
            if (data != null)
            {
                data.ForEach(o =>
                {
                    if (o.UserNavigation != null)
                    {
                        o.UserNavigation.Orders = null;
                        o.UserNavigation.Orders = null;
                    }
                });
            }
            return data;
        }

        //new
        public IEnumerable<Order> GetOrdersWithNavigationProperties()
        {
            IQueryable<Order> query = Context.Orders;
            query = query
                .Include(o => o.CartLines)
                .ThenInclude(c => c.ProductNavigation)
                .ThenInclude(p => p.Supplier)
                .Include(o => o.PaymentNavigation);

            List<Order> data = query.ToList();
            data.ForEach(o =>
            {
                if (o.CartLines != null)
                {
                    foreach (CartLine c in o.CartLines)
                    {
                        if (c.ProductNavigation != null)
                        {
                            c.ProductNavigation.Ratings = null;
                            c.ProductNavigation.CartLines = null;
                            if (c.ProductNavigation.Supplier != null)
                            {
                                c.ProductNavigation.Supplier.Products = null;
                            }
                        }
                        c.OrderNavigation = null;
                    }
                }
                if (o.PaymentNavigation != null)
                {
                    o.PaymentNavigation.OrderNavigation = null;
                }
            });
            return data;
        }






        public decimal GetTotalPriceAndPersist(long orderId, List<CartLine> cartLines)
        {
            bool persist = true;
            decimal Total = GetTotalPrice(cartLines);
            var dbRecord = Find(orderId);
            dbRecord.Total = Total;
            return base.Update(dbRecord, persist);
        }

        public Order GetOrder(long orderId)
        {
            IEnumerable<Order> orders = Context.Orders
                .Include(o => o.CartLines)
                .Include(o => o.PaymentNavigation)
                .Include(o => o.UserNavigation.NormalizedUserName);
            return Find(orderId);
        }

        public Order GetOrderWithCartLines(long orderId)
        {
            Order result = Table
                .Include(o => o.CartLines)
                .FirstOrDefault(o => o.Id == orderId);
            if (result != null)
            {
                if (result.CartLines != null)
                {
                    foreach (CartLine r in result.CartLines)
                    {
                        r.ProductNavigation = null;
                        r.OrderNavigation = null;
                    }
                }
            }
            return result;
        }

        public IEnumerable<Order> GetOrders()
        {
            return GetAll();
        }

        ////new
        //public IEnumerable<Order> GetOrdersWithPayment()
        //{
        //    IQueryable<Order> query = Context.Orders;
        //    query = query
        //        .Include(o => o.CartLines)
        //        .Include(o => o.PaymentNavigation);

        //        List<Order> data = query.ToList();
        //        data.ForEach(o => {
        //            if (o.CartLines != null)
        //            {
        //                foreach (CartLine c in o.CartLines)
        //                {
        //                    c.ProductNavigation = null;
        //                }
        //                foreach (CartLine c in o.CartLines)
        //                {
        //                    c.OrderNavigation = null;
        //                }
        //            }
        //            if (o.PaymentNavigation != null)
        //            {
        //                o.PaymentNavigation.OrderNavigation = null;
        //            }
        //        });
        //        return data;
        //}



        public IEnumerable<Order> GetOrdersByUserId(string userId)
            => Context.Orders.Where(o => o.UserId == userId);

        public void MarkShipped(long id)
        {
            Order order = Find(id);
            if (order != null)
            {
                order.Shipped = true;
                SaveChanges();
            }
        }
    }
}
