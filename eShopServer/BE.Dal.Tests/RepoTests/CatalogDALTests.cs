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
    public class CatalogDALTests : RepoTestsBase
    {
        private readonly IProductRepo _productRepo;
        public CatalogDALTests()
        {
            _productRepo = new ProductRepo(Context);
            Context.ProductId = 1;
            LoadDatabase();
        }
        public override void Dispose()
        {
            _productRepo.Dispose();
        }

        //Catalog01
        //(CategoryId, ProductCount)
        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public void ShouldGetAllProductsForACategory(int catId, int productCount)
        {
            var prods = _productRepo.GetProductsByCategory(catId).ToList();
            Assert.Equal(productCount, prods.Count());
        }

        //Catalog02
        [Fact]
        public void ShouldRetrieveDetailsOfAProduct()
        {
            var prod = _productRepo.GetProductWithNavigationProperties(Context.ProductId);
            Assert.Equal("Category1", prod.CategoryNavigation.Name);
            Assert.Equal("NameProduct1", prod.Name);
            Assert.Equal(275M, prod.Price);
            Assert.Equal(7, prod.UnitsInStock);
            Assert.Equal("Description1", prod.Description);
            Assert.Equal("NameSupplier1", prod.Supplier.Name);
            Assert.Equal("AddressSupplier1", prod.Supplier.Address);
            Assert.Equal("CitySupplier1", prod.Supplier.City);
            Assert.Equal(4, prod.Ratings[0].Stars);
        }
    }
}
