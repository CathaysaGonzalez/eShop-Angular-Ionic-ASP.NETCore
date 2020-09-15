using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using BE.Dal.EfStructures;
using BE.Dal.Initialization;
using BE.Models.Entities;
using BE.Service.Tests.TestClasses.Base;
using Newtonsoft.Json;
using Xunit;

namespace BE.Service.Tests.TestClasses
{
    [Collection("Service Testing")]
    public class CatalogServiceTests : BaseTestClass
    {
        private readonly string RootAddressProductController = String.Empty;
        public CatalogServiceTests()
        {
            RootAddressProductController = "api/products";
            SampleDataInitializer.InitializeData(new BEIdentityContextFactory().CreateDbContext(null));
        }

        //Catalog01
        [Fact]
        public async void SearchShouldReturnProductsByCategory()
        {
            //https://localhost:5001/api/products?category=Category1
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressProductController}/search?category=Category1");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(jsonResponse);
                Assert.Single(products);
                Assert.Equal("Description1", products[0].Description);
                Assert.Equal(275M, products[0].Price);
            }
        }

        [Fact]
        public async void SearchShouldNotReturnAnyProductsIfCategoryDoesNotExist()
        {
            //https://localhost:5001/api/products?category=Category4
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressProductController}/search?category=Category4");
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

        //Catalog02
        [Fact]
        public async void ShouldGetOneProduct()
        {
            //Get One Product: https://localhost:5001/api/products/2
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressProductController}/2");
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(jsonResponse);
                Assert.Equal("NameProduct2", product.Name);
                Assert.Equal(2, product.CategoryId);
            }
        }

        [Fact]
        public async void ShouldNotReturnAnyProductAsProductIdDoesNotExist()
        {
            //Get One Product: https://localhost:5001/api/products/4
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressProductController}/4");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

            }
        }
    }
}
