using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BE.Dal.EfStructures;
using BE.Models.Entities;

namespace BE.Dal.Initialization
{
    public static class SampleDataInitializer
    {
        public static void DropAndCreateDatabase(BEIdentityContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }

        internal static void ResetIdentity(BEIdentityContext context)
        {
            var tables = new[] { "Suppliers", "Products", "Ratings", "Orders", "CartLines", "Payments", "Categories" };
            foreach (var itm in tables)
            {
                var rawSqlString = $"DBCC CHECKIDENT (\"dbo.{itm}\", RESEED, 0);";
                context.Database.ExecuteSqlRaw(rawSqlString);
            }
        }

        public static void ClearData(BEIdentityContext context)
        {
            context.Database.ExecuteSqlRaw("Delete from dbo.Suppliers");
            context.Database.ExecuteSqlRaw("Delete from dbo.Orders");
            context.Database.ExecuteSqlRaw("Delete from dbo.Categories");
            ResetIdentity(context);
        }


        internal static void SeedData(BEIdentityContext context)
        {
            try
            {
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(SampleData.GetAllCategories());
                    context.SaveChanges();
                }
                if (!context.Suppliers.Any())
                {
                    var cat1 = context.Categories.FirstOrDefault();
                    var cat2 = context.Categories.Skip(1).FirstOrDefault();
                    context.Suppliers.AddRange(SampleData.GetSuppliers(new List<Category> { cat1, cat2 }));
                    context.SaveChanges();
                }

                if (!context.Orders.Any())
                {
                    var prod1 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.FirstOrDefault();
                    var prod2 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(1).FirstOrDefault();
                    context.Orders.AddRange(SampleData.GetAllOrders(new List<Product> { prod1, prod2 }));
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void InitializeData(BEIdentityContext context)
        {
            //Ensure the database exists and is up to date
            context.Database.Migrate();
            ClearData(context);
            SeedData(context);
        }

    }


}
