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
    public class UserAccountServiceTests : BaseTestClass
    {
        private readonly string RootAddressAdminController = String.Empty;
        private readonly string RootAddressAccountController = String.Empty;
        public UserAccountServiceTests()
        {
            RootAddressAccountController = "api/account";
            RootAddressAdminController = "api/admin";
            SampleDataInitializer.InitializeData(new BEIdentityContextFactory().CreateDbContext(null));
        }

        public override void Dispose()
        {
        }

        //UserAccount01
        [Fact]
        public async void ShouldDoLoginWithExistingUser()
        {
            using (var client = new HttpClient())
            {
                var responseLogIn = await client.PostAsync($"{ServiceAddress}{RootAddressAccountController}/login",
                    new StringContent("{\"name\":\"user\",\"password\":\"password\"}", Encoding.UTF8, "application/json"));
                Assert.True(responseLogIn.IsSuccessStatusCode, responseLogIn.ReasonPhrase);
            }
        }

        //UserAccount02
        [Fact]
        public async void ShouldDoNotLoginWithInvalidUser()
        {
            using (var client = new HttpClient())
            {
                var responseLogIn = await client.PostAsync($"{ServiceAddress}{RootAddress}/login",
                    new StringContent("{\"name\":\"user\"}", Encoding.UTF8, "application/json"));
                Assert.False(responseLogIn.IsSuccessStatusCode, responseLogIn.ReasonPhrase);
                //Assert.Equal(HttpStatusCode.BadRequest, responseLogIn.StatusCode);
                Assert.Equal(HttpStatusCode.NotFound, responseLogIn.StatusCode);
                //Assert.Equal(HttpStatusCode.InternalServerError, responseLogIn.StatusCode);
            }
        }

        //UserAccount02
        [Fact]
        public async void ShouldDoNotLoginWithNonExistingUser()
        {
            using (var client = new HttpClient())
            {
                var responseLogIn = await client.PostAsync($"{ServiceAddress}{RootAddressAccountController}/login",
                    new StringContent("{\"name\":\"nonuser\",\"password\":\"password\"}", Encoding.UTF8, "application/json"));
                Assert.False(responseLogIn.IsSuccessStatusCode, responseLogIn.ReasonPhrase);
                Assert.Equal(HttpStatusCode.BadRequest, responseLogIn.StatusCode);
            }
        }

        //UserAccount03
        [Fact]
        public async void ShouldDoLogout()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsUser(ServiceAddress, $"api/account/login", client);
                var targetLogout = $"{ServiceAddress}{RootAddressAccountController}/logout";
                var responseLogOut = await client.PostAsync(targetLogout, null);
                Assert.True(responseLogOut.IsSuccessStatusCode, responseLogOut.ReasonPhrase);
            }
        }

        //UserAccount04
        [Fact]
        public async void ShouldGetOrdersForCurrentCustomer()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsUser(ServiceAddress, $"api/account/login", client);
                await OrderTestHelpers.Create3OrdersForCurrentUser(ServiceAddress, "api/orders/user", client);
                var response = await client.GetAsync($"{ServiceAddress}api/orders/user");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<Order>>(jsonResponse);
                Assert.Equal(3, orders.Count);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        //UserAccount05
        [Fact]
        public async void ShouldEditCustomerProfileAsUser()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsUser(ServiceAddress, $"api/account/login", client);
                var targetUrl = $"{ServiceAddress}{RootAddressAdminController}/CurrentUser";
                var response = await client.PostAsync(targetUrl,
                    new StringContent("{\"name\":\"user\",\"password\":\"password\",\"email\":\"newemail\"}",
                        Encoding.UTF8, "application/json"));
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
                //Validate new password
                var user = await AdminTestHelpers.GetUser("CurrentUser", ServiceAddress, RootAddressAdminController, client);
                Assert.Equal("newemail", user.Email);
                var responseDefault = await client.PostAsync(targetUrl,
                    new StringContent("{\"name\":\"user\",\"password\":\"password\",\"email\":\"user@example.com\"}",
                        Encoding.UTF8, "application/json"));
                Assert.True(responseDefault.IsSuccessStatusCode, response.ReasonPhrase);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        //UserAccount06
        [Fact]
        public async void ShouldNotEditCustomerProfileWithInvalidData()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsUser(ServiceAddress, $"api/account/login", client);
                var targetUrl = $"{ServiceAddress}{RootAddressAdminController}/CurrentUser";
                var response = await client.PostAsync(targetUrl,
                    new StringContent("{\"name\":\"user\"}",
                        Encoding.UTF8, "application/json"));
                Assert.False(response.IsSuccessStatusCode, response.ReasonPhrase);
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }
    }
}
