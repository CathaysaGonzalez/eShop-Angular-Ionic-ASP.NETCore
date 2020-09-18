using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using BE.Dal.EfStructures;
using BE.Dal.Initialization;
using BE.Models.Entities;
using BE.Service.Tests.Helpers;
using BE.Service.Tests.TestClasses.Base;
using Newtonsoft.Json;
using Xunit;

namespace BE.Service.Tests.TestClasses
{
    [Collection("Service Testing")]
    public class SearchServiceTests : BaseTestClass
    {

        private readonly string RootAddressAdminController = String.Empty;
        private readonly string RootAddressOrderController = String.Empty;
        private readonly string RootAddressProductController = String.Empty;
        public SearchServiceTests()
        {
            RootAddressProductController = "api/products";
            RootAddressAdminController = "api/admin";
            RootAddressOrderController = "api/orders";
            SampleDataInitializer.InitializeData(new BEIdentityContextFactory().CreateDbContext(null));
        }

        //Search01
        [Fact]
        public async void SearchShouldReturnProducts()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressProductController}?search=Product1");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(jsonResponse);
                Assert.Single(products);
                Assert.Equal("Description1", products[0].Description);
                Assert.Equal(275M, products[0].Price);
            }
        }

        //Search02
        [Fact]
        public async void SearchShouldNotReturnAnyProduct()
        {
            //https://localhost:5001/api/products?search=Product3
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressProductController}?search=Product4");
                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(jsonResponse);
                Assert.Null(products);
            }
        }

        //Search05
        [Fact]
        public async void ShouldSearchForAUserName()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressAdminController}/search/user");
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<AppUser>(jsonResponse);
                Assert.Equal("user@example.com",user.Email);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        [Fact]
        public async void ShouldNotSearchForAUserNameAsUserIsNotAnAdministrator()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsUser(ServiceAddress, $"api/account/login", client);
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressAdminController}/search/user");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                //Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        //Search06
        [Fact]
        public async void ShouldNotFindAnyUserThatMatchesANonExistingUserName()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressAdminController}/search/nonuser");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                //Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        //Search07
        [Fact]
        public async void ShouldFindAnOrderAsAdministrator()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressOrderController}/1");
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(jsonResponse);
                Assert.Equal(1, order.Id);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        //Search08
        [Fact]
        public async void ShouldNotFindAnOrderThatHasAnInvalidId()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressOrderController}/5");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
 
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }
    }
}
