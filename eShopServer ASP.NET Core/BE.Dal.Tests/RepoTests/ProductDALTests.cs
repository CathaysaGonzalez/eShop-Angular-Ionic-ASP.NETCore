using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BE.Dal.EfStructures;
using BE.Dal.Repos;
using BE.Dal.Repos.Interfaces;
using BE.Dal.Tests.RepoTests.Base;
using BE.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace BE.Dal.Tests.RepoTests
{
    [Collection("BE.Dal")]
    public class ProductDALTests : RepoTestsBase
    {
        private readonly IProductRepo _productRepo;

        public ProductDALTests()
        {
            _productRepo = new ProductRepo(Context);
            Context.ProductId = 1;
            LoadDatabase();
        }

        public override void Dispose()
        {
            _productRepo.Dispose();
        }

        //AdminAccount06
        [Fact]
        public void ShouldUpdateProduct1()
        {
            var item = _productRepo.Find(1);
            item.Name = "NewProduct";
            _productRepo.Update(item);
            var products = _productRepo.GetAll().ToList();
            Assert.Equal(3, products.Count());
            Assert.Equal("NewProduct", products[0].Name);
        }

        //AdminAccount08
        [Fact]
        public void ShouldDeleteAProduct()
        {
            var item = _productRepo.Find(1);
            _productRepo.Context.Entry(item).State = EntityState.Detached;
            _productRepo.Delete(item);
            var product = _productRepo.Find(Context.ProductId);
            Assert.Null(product);
        }

        //AdminAccount09
        [Fact]
        public void ShouldRetrieveDetailsOfAProduct()
        {
            var item = _productRepo.Find(1);
            Assert.Equal(1, item.Id);
            Assert.Equal("NameProduct1", item.Name);
            Assert.Equal(275M, item.Price);
            Assert.Equal(1, item.SupplierId);
            Assert.Equal(7, item.UnitsInStock);
        }

        //AdminAccount09
        [Fact]
        public void ShouldRetrieveAllItemsFromProductTable()
        {
            var products = _productRepo.GetAll().ToList();
            Assert.Equal(3, products.Count());
            Assert.Equal(1, products[0].Id);
            Assert.Equal(2, products[1].Id);
            Assert.Equal(3, products[2].Id);
        }

        [Fact]
        public void AddAnItemToProductTable()
        {
            var product = new Product
            {
                Name = "Product4",
                CategoryId = 1,
                Description = "Description1",
                Price = 11,
                SupplierId = 1,
            };
            var count = _productRepo.Add(product);
            Assert.Equal(1, count);
            Assert.Equal(4, product.Id);
            Assert.Equal(4, _productRepo.Table.Count());
        }

        [Fact]
        public void ShouldAddAProduct()
        {
            var product = new Product
            {
                Name = "Product7",
                Description = "Description7",
                Price = 20M,
                SupplierId = 1,
                UnitsInStock = 20,
                CategoryId = 1
            };
            _productRepo.Add(product);
            var products = _productRepo.GetAll().ToList();
            Assert.Equal(4, products[3].Id);
            Assert.Equal("Product7", products[3].Name);
            Assert.Equal("Description7", products[3].Description);
            Assert.Equal(20M, products[3].Price);
            Assert.Equal(20, products[3].UnitsInStock);
            Assert.Equal(1, products[3].CategoryId);
        }

    }
}

