using System;
using System.Collections.Generic;
using System.Linq;
using BE.Dal.Repos.Base;
using BE.Models.Entities;

namespace BE.Dal.Repos.Interfaces
{
    public interface IProductRepo : IRepo<Product>
    {
        IEnumerable<Product> GetProductsWithNavigationProperties();
        Product GetProductWithNavigationProperties(long id);
        IQueryable<Product> SearchProducts(string category, string search);
        List<Product> SearchProductsWithNavigationProperties(string category, string search, IQueryable<Product> query);
        int Update(Product entity, bool persist = true);
        IList<Product> GetProductsByCategory(int id);
    }
}