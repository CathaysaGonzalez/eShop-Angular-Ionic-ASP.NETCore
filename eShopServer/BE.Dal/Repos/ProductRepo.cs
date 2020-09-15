using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BE.Dal.EfStructures;
using BE.Dal.Repos.Base;
using BE.Dal.Repos.Interfaces;
using BE.Models.Entities;
using BE.Models.Entities.Base;
using Microsoft.EntityFrameworkCore.Query;

namespace BE.Dal.Repos
{
    public class ProductRepo : RepoBase<Product>,IProductRepo
    {
        public ProductRepo(BEIdentityContext context) : base(context)
        {
        }

        internal ProductRepo(DbContextOptions<BEIdentityContext> options) : base(options)
        {
        }
        public override void Dispose()
        {
          base.Dispose();
        }

        public IEnumerable<Product> GetProductsWithNavigationProperties()
        {
            IQueryable<Product> query = Context.Products;
            query = query
                .Include(p => p.Supplier)
                .Include(p => p.Ratings)
                .Include(p => p.CategoryNavigation);
            List<Product> data = query.ToList();
            data.ForEach(p => {
                if (p.Supplier != null)
                {
                    p.Supplier.Products = null;
                }
                if (p.Ratings != null)
                {
                    p.Ratings.ForEach(r => r.ProductNavigation = null);
                }
                if (p.CategoryNavigation != null)
                {
                    p.CategoryNavigation.Products = null;
                }
            });
            return data;
        }

        public Product GetProductWithNavigationProperties(long id)
        {
            Product result = Table
                .Include(p => p.Supplier).ThenInclude(s => s.Products)
                .Include(p => p.Ratings)
                .Include(p => p.CategoryNavigation)
                .FirstOrDefault(p => p.Id == id);
            if (result != null)
            {
                if (result.Supplier != null)
                {
                    result.Supplier.Products = result.Supplier.Products.Select(p =>
                        new Product
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            Price = p.Price,
                        });
                }
                if (result.Ratings != null)
                {
                    foreach (Rating r in result.Ratings)
                    {
                        r.ProductNavigation = null;
                    }
                }
                if (result.CategoryNavigation != null)
                {
                    result.CategoryNavigation.Products = null;
                }
            }
            return result;
        }

        public IQueryable<Product> SearchProducts(string category, string search)
        {
            IQueryable<Product> query = Context.Products;
            if (!string.IsNullOrWhiteSpace(category))
            {
                string catLower = category.ToLower();
                query = query.Where(p => p.CategoryNavigation.Name.ToLower().Contains(catLower));
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                string searchLower = search.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchLower) || p.Description.ToLower().Contains(searchLower));
            }
            return query;
        }

        public List<Product> SearchProductsWithNavigationProperties(string category, string search, IQueryable<Product> query)
        {
            query = query.Include(p => p.Supplier).Include(p => p.Ratings).Include(p => p.CategoryNavigation);
            List<Product> data = query.ToList();
            data.ForEach(p =>
            {
                if (p.Supplier != null)
                {
                    p.Supplier.Products = null;
                }

                if (p.Ratings != null)
                {
                    p.Ratings.ForEach(r => r.ProductNavigation = null);
                }

                if (p.CategoryNavigation != null)
                {
                    p.CategoryNavigation.Products = null;
                }
            });
            return data;
        }

        public override int Update(Product entity, bool persist = true)
        {
            var dbRecord = Find(entity.Id);
            dbRecord.Name = entity.Name;
            dbRecord.Description = entity.Description;
            dbRecord.Price = entity.Price;
            dbRecord.UnitsInStock = entity.UnitsInStock;
            dbRecord.ModelNumber = entity.ModelNumber;
            dbRecord.ModelName = entity.ModelName;
            dbRecord.CurrentPrice = entity.CurrentPrice;
            dbRecord.IsFeatured = entity.IsFeatured;
            dbRecord.SupplierId = entity.SupplierId;
            dbRecord.CategoryId = entity.CategoryId;
            dbRecord.ProductImage = entity.ProductImage;
            dbRecord.ProductImageLarge = entity.ProductImageLarge;
            dbRecord.ProductImageThumb = entity.ProductImageThumb;
            return base.Update(dbRecord, persist);
        }

        public IList<Product> GetProductsByCategory(int id) 
            => Table.Where(p => p.CategoryId == id)
            .Include(p => p.CategoryNavigation)
            .OrderBy(x => x.Name)
            .ToList();

    }
}
