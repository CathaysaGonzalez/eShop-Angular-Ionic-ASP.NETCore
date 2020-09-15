using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Dal.EfStructures;
using BE.Dal.Initialization;
using BE.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BE.Dal.Initialization
{
    public class SeedData
    {
        public static void SeedDataBase(BEIdentityContext context)
        {
            context.Database.Migrate();


            try
            {
                if (!context.Categories.Any())
                {
                    context.Categories.AddRange(Data.GetAllCategories());
                    context.SaveChanges();
                }

                if (!context.Suppliers.Any())
                {
                    var cat1 = context.Categories.FirstOrDefault();
                    var cat2 = context.Categories.Skip(1).FirstOrDefault();
                    var cat3 = context.Categories.Skip(2).FirstOrDefault();
                    var cat4 = context.Categories.Skip(3).FirstOrDefault();
                    var cat5 = context.Categories.Skip(4).FirstOrDefault();
                    context.Suppliers.AddRange(
                        Data.GetSuppliers(new List<Category> { cat1, cat2, cat3, cat4, cat5 }));
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
                    //var prod3 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(2).FirstOrDefault();
                    //var prod4 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(3).FirstOrDefault();
                    //var prod5 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(4).FirstOrDefault();
                    var prod6 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(5).FirstOrDefault();
                    var prod7 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(6).FirstOrDefault();
                    var prod8 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(7).FirstOrDefault();
                    var prod9 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(8).FirstOrDefault();
                    var prod10 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(9).FirstOrDefault();
                    var prod11 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(10).FirstOrDefault();
                    //var prod12 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(11).FirstOrDefault();
                    //var prod13 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(12).FirstOrDefault();
                    var prod14 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(13).FirstOrDefault();
                    //var prod15 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(14).FirstOrDefault();
                    //var prod16 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(15).FirstOrDefault();
                    var prod17 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(16).FirstOrDefault();
                    //var prod18 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(17).FirstOrDefault();
                    var prod19 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(18).FirstOrDefault();
                    var prod20 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(19).FirstOrDefault();
                    var prod21 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(20).FirstOrDefault();
                    //var prod22 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(21).FirstOrDefault();
                    //var prod23 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(22).FirstOrDefault();
                    //var prod24 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(23).FirstOrDefault();
                    //var prod25 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(24).FirstOrDefault();
                    var prod26 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(25).FirstOrDefault();
                    var prod27 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(26).FirstOrDefault();
                    var prod28 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(27).FirstOrDefault();
                    var prod29 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(28).FirstOrDefault();
                    //var prod30 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(29).FirstOrDefault();
                    //var prod31 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(30).FirstOrDefault();
                    //var prod32 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(31).FirstOrDefault();
                    //var prod33 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(32).FirstOrDefault();
                    //var prod34 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(33).FirstOrDefault();
                    //var prod35 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(34).FirstOrDefault();
                    //var prod36 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(35).FirstOrDefault();
                    //var prod37 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(36).FirstOrDefault();
                    var prod38 = context.Suppliers
                        .Include(c => c.Products).FirstOrDefault()?
                        .Products.Skip(37).FirstOrDefault();
                    //var prod39 = context.Suppliers
                    //    .Include(c => c.Products).FirstOrDefault()?
                    //    .Products.Skip(38).FirstOrDefault();



                    context.Orders.AddRange(Data.GetAllOrders(new List<Product>
                    {
                        prod1,
                        prod2,
                        //prod3,
                        //prod4, 
                        //prod5,
                        prod6,
                        prod7,
                        prod8,
                        prod9,
                        prod10,
                        prod11,
                        //prod12, 
                        //prod13,
                        prod14,
                        //prod15,
                        //prod16, 
                        prod17,
                        //prod18, 
                        prod19,
                        prod20,
                        prod21,
                        //prod22, 
                        //prod23, 
                        //prod24, 
                        //prod25,
                        prod26,
                        prod27,
                        prod28,
                        prod29,
                        //prod30,
                        //prod31, 
                        //prod32, 
                        //prod33, 
                        //prod34, 
                        //prod35,
                        //prod36, 
                        //prod37, 
                        prod38,
                        //prod39
                    }));
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
