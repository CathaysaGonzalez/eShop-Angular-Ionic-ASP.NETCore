using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using BE.Dal.EfStructures;
using BE.Dal.Initialization;
using BE.Models.Entities;
using BE.Models.ViewModels;
using BE.Service.Tests.Helpers;
using BE.Service.Tests.TestClasses.Base;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Newtonsoft.Json;
using Xunit;

namespace BE.Service.Tests.TestClasses
{
    [Collection("Service Testing")]
    public class AdminAccountServiceTests : BaseTestClass
    {
        private readonly string RootAddressAccountController = String.Empty;
        private readonly string RootAddressAdminController = String.Empty;
        private readonly string RootAddressOrderController = String.Empty;
        private readonly string RootAddressProductController = String.Empty;
        private readonly string RootAddressRoleAdminController = String.Empty;
        private readonly long _orderId = 1;
        private readonly long _productId = 1;

        public AdminAccountServiceTests()
        {
            RootAddressAdminController = "api/admin";
            RootAddressAccountController = "api/account";
            RootAddressProductController = "api/products";
            RootAddressOrderController = "api/orders";
            RootAddressRoleAdminController = "api/roleadmin";
            SampleDataInitializer.InitializeData(new BEIdentityContextFactory().CreateDbContext(null));
        }

        public override void Dispose()
        {
        }

        //AdminAccount01
        [Fact]
        public async void ShouldDoLoginWithExistingAdministrator()
        {
            using (var client = new HttpClient())
            {
                var responseLogIn = await client.PostAsync($"{ServiceAddress}{RootAddressAccountController}/login",
                    new StringContent("{\"name\":\"admin\",\"password\":\"secret\"}", Encoding.UTF8,
                        "application/json"));
                Assert.True(responseLogIn.IsSuccessStatusCode, responseLogIn.ReasonPhrase);
            }
        }

        //AdminAccount01
        [Fact]
        public async void ShouldDoNotLoginWithInvalidAdministrator()
        {
            using (var client = new HttpClient())
            {
                var responseLogIn = await client.PostAsync($"{ServiceAddress}{RootAddressAccountController}/login",
                    new StringContent("{\"name\":\"admin\"}", Encoding.UTF8, "application/json"));
                Assert.False(responseLogIn.IsSuccessStatusCode, responseLogIn.ReasonPhrase);
                Assert.Equal(HttpStatusCode.BadRequest, responseLogIn.StatusCode);
            }
        }

        //AdminAccount01
        [Fact]
        public async void ShouldDoNotLoginWithNonExistingAdministrator()
        {
            using (var client = new HttpClient())
            {
                var responseLogIn = await client.PostAsync($"{ServiceAddress}{RootAddressAccountController}/login",
                    new StringContent("{\"name\":\"nonadmin\",\"password\":\"secret\"}", Encoding.UTF8,
                        "application/json"));
                Assert.False(responseLogIn.IsSuccessStatusCode, responseLogIn.ReasonPhrase);
                Assert.Equal(HttpStatusCode.BadRequest, responseLogIn.StatusCode);
            }
        }

        //AdminAccount02
        [Fact]
        public async void ShouldEditCustomerProfile()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                //var user2 = await AdminTestHelpers.GetUser("user", ServiceAddress, RootAddress, client);
                //Assert.Equal("user@example.com", user2.Email);
                var targetUrl = $"{ServiceAddress}{RootAddressAdminController}/User";
                var response = await client.PostAsync(targetUrl,
                    new StringContent("{\"name\":\"user\",\"password\":\"password\",\"email\":\"newemail\"}",
                        Encoding.UTF8, "application/json"));
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
                //Validate new password
                var user = await AdminTestHelpers.GetUser("user", ServiceAddress, RootAddressAdminController, client);
                Assert.Equal("newemail", user.Email);
                var responseDefault = await client.PostAsync(targetUrl,
                    new StringContent("{\"name\":\"user\",\"password\":\"password\",\"email\":\"user@example.com\"}",
                        Encoding.UTF8, "application/json"));
                Assert.True(responseDefault.IsSuccessStatusCode, response.ReasonPhrase);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        //AdminAccount03
        [Fact]
        public async void ShouldNotEditCustomerProfileWithIncompleteData()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                var targetUrl = $"{ServiceAddress}{RootAddressAdminController}/User";
                var response = await client.PostAsync(targetUrl,
                    new StringContent("{\"name\":\"user\"}",
                        Encoding.UTF8, "application/json"));
                Assert.False(response.IsSuccessStatusCode, response.ReasonPhrase);
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        //AdminAccount04
        [Fact]
        public async void ShouldDeleteAndCreateCustomer()
        {
            // Remove User: https://localhost:5001/api/admin/{id} HTTPDelete
            // https://localhost:5001/api/admin/user
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                //Delete user
                var targetUrl = $"{ServiceAddress}{RootAddressAdminController}/User";
                var response = await client.DeleteAsync(targetUrl);
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
                //Create user
                var targetUrl2 = $"{ServiceAddress}{RootAddressAdminController}";
                var responseDefault = await client.PostAsync(targetUrl2,
                    new StringContent("{\"name\":\"user\",\"password\":\"password\",\"email\":\"user@example.com\"}",
                        Encoding.UTF8, "application/json"));
                Assert.True(responseDefault.IsSuccessStatusCode, responseDefault.ReasonPhrase);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        [Fact]
        public async void ShouldFailIfBadProductId()
        {
            //Get One Product: https://localhost:5001/api/product/100
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressAdminController}/100");
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                //Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            }
        }

        //AdminAccount06
        [Fact]
        public async void ShouldUpdateProduct()
        {
            //Change Product(Price): http://localhost:5001/api/shoppingcart/{customerId}/{productId} HTTPPut
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                var product = await ProductTestHelpers.GetProduct(_productId, ServiceAddress, RootAddressProductController);
                product.Price = 44;
                var json = JsonConvert.SerializeObject(product);
                //Actualiza producto
                //Name = "ReplaceProduct"
                // PUT https://localhost:5001/api/products/1
                var targetUrl = $"{ServiceAddress}{RootAddressProductController}/{_productId}";
                var response = await client.PutAsync(targetUrl, new StringContent(json, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception(response.StatusCode.ToString());
                }

                Assert.True(response.IsSuccessStatusCode);
                // validate product was updated
                var updatedProduct =
                    await ProductTestHelpers.GetProduct(_productId, ServiceAddress, RootAddressProductController);
                Assert.Equal(44, updatedProduct.Price);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        //AdminAccount08
        [Fact]
        public async void ShouldDeleteProduct()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                var product =
                    await ProductTestHelpers.GetProduct(_productId, ServiceAddress, RootAddressProductController);
                var targetUrl = $"{ServiceAddress}{RootAddressProductController}/{_orderId}";
                var response = await client.DeleteAsJsonAsync(targetUrl, product);
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
                List<Product> products =
                    await ProductTestHelpers.GetProducts(ServiceAddress, RootAddressProductController);
                Assert.Equal(2, products.Count);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        //AdminAccount09
        [Fact]
        public async void ShouldViewProduct()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressProductController}/{_productId}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var product = JsonConvert.DeserializeObject<Product>(jsonResponse);
                Assert.Equal("NameProduct1", product.Name);
            }
        }

        [Fact]
        public async void ShouldAddProduct()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                var targetUrl = $"{ServiceAddress}{RootAddressProductController}";
                //Añade nuevo producto de Supplier y Categoria existentes
                //POST
                //https://
                var response = await client.PostAsync(targetUrl,
                    new StringContent(
                        "{\"Name\":\"Product1\",\"CategoryId\":1,\"Description\":\"Description1\",\"Price\":11.0,\"SupplierId\":1}",
                        Encoding.UTF8, "application/json"));
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
                List<Product> products =
                    await ProductTestHelpers.GetProducts(ServiceAddress, RootAddressProductController);
                // validate the product was added
                Assert.Equal(4, products.Count);
                var product = products[products.Count - 1];
                Assert.Equal(4, product.Id);
                Assert.Equal("Product1", product.Name);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        [Fact]
        public async void ShouldGetAllOrders()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressOrderController}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<Order>>(jsonResponse);
                Assert.Equal("NameCustomer2", orders[0].Name);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        //AdminAccount10
        [Fact]
        public async void ShouldUpdateAnOrder()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                var order = await OrderTestHelpers.GetOrder(_orderId, ServiceAddress, RootAddressOrderController,
                    client);
                order.Address = "newAddress";
                var json = JsonConvert.SerializeObject(order);
                var targetUrl = $"{ServiceAddress}{RootAddressOrderController}/{_orderId}";
                var response = await client.PutAsync(targetUrl,
                    new StringContent(json, Encoding.UTF8, "application/json"));
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
                var updatedOrder =
                    await OrderTestHelpers.GetOrder(_orderId, ServiceAddress, RootAddressOrderController, client);
                Assert.Equal("newAddress", updatedOrder.Address);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        //AdminAccount10
        [Fact]
        public async void ShouldMarkAnOrderAsShipped()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                var order = await OrderTestHelpers.GetOrder(_orderId, ServiceAddress, RootAddressOrderController,
                    client);
                var targetUrl = $"{ServiceAddress}{RootAddressOrderController}/shipped/{_orderId}";
                var response = await client.PostAsync(targetUrl, null);
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
                var updatedOrder =
                    await OrderTestHelpers.GetOrder(_orderId, ServiceAddress, RootAddressOrderController, client);
                Assert.True(updatedOrder.Shipped);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        //AdminAccount12
        [Fact]
        public async void ShouldDeleteOrder()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                var targetUrl = $"{ServiceAddress}{RootAddressOrderController}/{_orderId}";
                var response = await client.DeleteAsync(targetUrl);
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
                List<Order> orders =
                    await OrderTestHelpers.GetOrders(ServiceAddress, RootAddressProductController, client);
                Assert.Single(orders);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        //AdminAccount13
        [Fact]
        public async void ShouldGetOrder()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressOrderController}/{_orderId}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Order>(jsonResponse);
                Assert.Equal("NameCustomer2", order.Name);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        [Fact]
        public async void ShouldTestRoles()
        {
            using (var client = new HttpClient())
            {
                {
                    await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);

                    //Test: CreateUser AdminController
                    //Create user
                    var targetUrl1 = $"{ServiceAddress}{RootAddressAdminController}/create";
                    var response1 = await client.PostAsync(targetUrl1,
                        new StringContent(
                            "{\"name\":\"user2\",\"password\":\"password\",\"email\":\"user2@example.com\"}",
                            Encoding.UTF8, "application/json"));
                    Assert.True(response1.IsSuccessStatusCode, response1.ReasonPhrase);

                    //Test: CreateRole RoleAdminController
                    //Create new role
                    var targetUrl2 = $"{ServiceAddress}{RootAddressRoleAdminController}/create?name=users2";
                    var response2 = await client.PostAsync(targetUrl2, null);
                    Assert.True(response2.IsSuccessStatusCode, response2.ReasonPhrase);

                    //Test: EditUsersForRole(post) RoleAdminController
                    //Add a user to a role
                    object data = new RoleModification()
                    {
                        RoleName = "users2",
                        IdsToAdd = new string[]
                        {
                            "user2"
                        }
                    };
                    var myContent = JsonConvert.SerializeObject(data);
                    var targetUrl3 = $"{ServiceAddress}{RootAddressRoleAdminController}";
                    var response3 = await client.PostAsync(targetUrl3,
                        new StringContent(myContent, Encoding.UTF8, "application/json"));
                    Assert.True(response3.IsSuccessStatusCode, response3.ReasonPhrase);

                    //Test: ViewUsersForRole(id)(get) RoleAdminController
                    //Return users for a role
                    var response4 = await client.GetAsync($"{ServiceAddress}{RootAddressRoleAdminController}/users2");
                    Assert.True(response4.IsSuccessStatusCode, response4.ReasonPhrase);

                    //Test: EditUsersForRole(post) RoleAdminController
                    //Delete a user from a role
                    object data2 = new RoleModification()
                    {
                        RoleName = "users2",
                        IdsToDelete = new string[]
                        {
                                "user2"
                        }
                    };
                    var myContent2 = JsonConvert.SerializeObject(data2);
                    var targetUrl5 = $"{ServiceAddress}{RootAddressRoleAdminController}";
                    var response5 = await client.PostAsync(targetUrl5,
                        new StringContent(myContent2, Encoding.UTF8, "application/json"));
                    Assert.True(response5.IsSuccessStatusCode, response5.ReasonPhrase);

                    //Test: Delete RoleAdminController
                    //Delete a role
                    var targetUrl6 = $"{ServiceAddress}{RootAddressRoleAdminController}?id=users2";
                    var response6 = await client.DeleteAsync(targetUrl6);
                    Assert.True(response6.IsSuccessStatusCode, response6.ReasonPhrase);

                    //Test: DeleteRole(id) AdminController
                    //Delete user
                    var targetUrl = $"{ServiceAddress}{RootAddressAdminController}/user2";
                    var response = await client.DeleteAsync(targetUrl);
                    Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);

                    await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
                }
            }
        }

        [Fact]
        public async void ShouldSearchUser()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsAdmin(ServiceAddress, $"api/account/login", client);
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressAdminController}/user");
                Assert.True(response.IsSuccessStatusCode);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }
    }
}


