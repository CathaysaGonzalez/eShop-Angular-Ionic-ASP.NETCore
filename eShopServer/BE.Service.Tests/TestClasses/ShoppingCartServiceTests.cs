using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ShoppingCartServiceTests : BaseTestClass
    {
        private readonly string RootAddressCartLineController = String.Empty;
        private long _orderId = 1;
        private long _cartLineId = 1;
        private long _productId = 1;

        public ShoppingCartServiceTests()
        {
            RootAddressCartLineController = "api/Cartline";
            SampleDataInitializer.InitializeData(new BEIdentityContextFactory().CreateDbContext(null));
        }

        [Fact]
        public async void ShouldGetOneCartLine()
        {
            //Get One CartLine: http://localhost:5001/api/cartline/{cartlineId}
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressCartLineController}/1");
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var cartLine = JsonConvert.DeserializeObject<CartLine>(jsonResponse);
                Assert.Equal(2, cartLine.ProductId);
                Assert.Equal(4, cartLine.Quantity);
            }
        }

        [Fact]
        public async void ShouldFailGettingCartLineIfBadCartLineId()
        {
            //Get One CartLine: http://localhost:5001/api/cartline/{cartlineId}
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressCartLineController}/100");
                Assert.False(response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                //Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            }
        }

        //ShoppingCart02
        [Fact]
        public async void ShouldRetrieveAllItemsFromShoppingCart()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddressCartLineController}/order/{_orderId}");
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var cartLines = JsonConvert.DeserializeObject<List<CartLine>>(jsonResponse);
                Assert.Equal(2, cartLines[0].ProductId);
                Assert.Equal(4, cartLines[0].Quantity);
                Assert.Equal(1, cartLines[1].ProductId);
                Assert.Equal(3, cartLines[1].Quantity);
            }
        }

        //ShoppingCart03
        [Fact]
        public async void ShouldAddNewProductToTheOrder()
        {
            // Add to Order: http://localhost:5001/api/cartline/{_orderId} HTTPPost
            // http://localhost:5001/api/cartline/1 {"ProductId":3,"Quantity":1,"OrderId":1}
            using (var client = new HttpClient())
            {
                var targetUrl = $"{ServiceAddress}{RootAddressCartLineController}/{_orderId}";
                var response = await client.PostAsync(targetUrl,
                    new StringContent("{\"productId\":3,\"quantity\":1,\"orderId\":1}",
                        Encoding.UTF8, "application/json"));
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
            }
            // validate the cart was added
            List<CartLine> cartLines = await CartLineTestHelpers.GetCartLines(_orderId, ServiceAddress, "api/cartline/order");
            Assert.Equal(3, cartLines.Count);
            var cartLine = cartLines[cartLines.Count - 1];
            Assert.Equal(3, cartLine.ProductId);
            Assert.Equal(1, cartLine.Quantity);
        }

        //ShoppingCart04
        [Fact]
        public async void ShouldCalculateTotalOfOrder()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsUser(ServiceAddress, $"api/account/login", client);
                object data = new Order
                {
                    Name = "NameCustomer1",
                    Address = "AddressCustomer1",
                    Shipped = false,
                    CartLines = new List<CartLine>
                    {
                        new CartLine
                        {
                            ProductId = 1,
                            Quantity = 1,
                        },
                        new CartLine
                        {
                            ProductId = 2,
                            Quantity = 1,
                        }
                    },
                    PaymentNavigation = new Payment()
                    {
                        CardNumber = "111",
                        CardExpiry = "222",
                        CardSecurityCode = "333"
                    }
                };
                var myContent = JsonConvert.SerializeObject(data);
                var targetUrl = $"{ServiceAddress}api/orders";
                var response = await client.PostAsync(targetUrl, new StringContent(myContent, Encoding.UTF8, "application/json"));
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
                // validate the order was added
                List<Order> orders = await OrderTestHelpers.GetOrders(ServiceAddress, $"api/orders", client);
                var order = orders[orders.Count - 1];
                Assert.Equal(323.95M, order.Total);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }

        //ShoppingCart05
        [Fact]
        public async void ShouldDeleteRecordInTheCart()
        {
            // Remove Cart Item: https://localhost:5001/api/cartline/{id} HTTPDelete
            // https://localhost:5001/api/cartline/1
            var cartRecord = await CartLineTestHelpers.GetCartLine(_cartLineId, ServiceAddress, RootAddressCartLineController);
            using (var client = new HttpClient())
            {
                var targetUrl = $"{ServiceAddress}{RootAddressCartLineController}/{cartRecord.Id}";
                var response = await client.DeleteAsJsonAsync(targetUrl, cartRecord);
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
            }
            // validate the cart item was removed
            Order order = await CartLineTestHelpers.GetOrder(_orderId, ServiceAddress, "api/orders");
            Assert.Single(order.CartLines);
            Assert.Equal(825M,order.Total);
        }

        //ShoppingCart05
        [Fact]
        public async void ShouldRemoveRecordOnAddIfQuantityBecomesZero()
        {
            // https://localhost:5001/api/cartline/{orderId} HTTPPost
            // Initial data: {"ProductId":2,"Quantity":4,"OrderId":1} NumberOfRecords: 2
            using (var client = new HttpClient())
            {
                var quantity = -4;
                var targetUrl = $"{ServiceAddress}{RootAddressCartLineController}/{_orderId}";
                var response = await client.PostAsync(targetUrl,
                    new StringContent("{" + $"\"ProductId\":2,\"Quantity\":{quantity},\"OrderId\":{_orderId}" + "}",
                        Encoding.UTF8, "application/json"));
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
            }
            // validate the cart line was removed
            List<CartLine> cartLines = await CartLineTestHelpers.GetCartLines(_orderId, ServiceAddress, "api/cartline/order");
            Assert.Single(cartLines);
        }

        //ShoppingCart06
        [Fact]
        public async void ShouldUpdateCartLineIfAlreadyExistsAndQuantityIncrementsByOne()
        {
            // https://localhost:5001/api/cartline/{orderId} HTTPPost
            // Initial data: {"ProductId":1,"Quantity":3,"_orderId":1} NumberOfRecords: 2
            using (var client = new HttpClient())
            {
                var productId = 1;
                var quantity = 1;
                var targetUrl = $"{ServiceAddress}{RootAddressCartLineController}/{_orderId}";
                var response = await client.PostAsync(targetUrl,
                    new StringContent("{" + $"\"ProductId\":{productId},\"Quantity\":{quantity},\"OrderId\":{_orderId}" + "}",
                        Encoding.UTF8, "application/json"));
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
            }
            // validate units of the cart line
            List<CartLine> cartLines =
                await CartLineTestHelpers.GetCartLines(_orderId, ServiceAddress, "api/cartline/order");
            Assert.Equal(2, cartLines.Count);
            var cartRecord = cartLines[cartLines.Count - 1];
            Assert.Equal(4, cartRecord.Quantity);
        }

        //ShoppingCart07
        [Fact]
        public async void ShouldUpdateCartLineIfAlreadyExistsAndQuantityDecrementsByOne()
        {
            // https://localhost:5001/api/cartline/{orderId} HTTPPost
            // Initial data: {"ProductId":1,"Quantity":3,"OrderId":1} NumberOfRecords: 2
            using (var client = new HttpClient())
            {
                var productId = 1;
                var quantity = -1;
                var targetUrl = $"{ServiceAddress}{RootAddressCartLineController}/{_orderId}";

                var response = await client.PostAsync(targetUrl,
                    new StringContent("{" + $"\"ProductId\":{productId},\"Quantity\":{quantity},\"OrderId\":{_orderId}" + "}",
                        Encoding.UTF8, "application/json"));
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);

                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
            }
            // validate units of the cart line
            List<CartLine> cartLines =
                await CartLineTestHelpers.GetCartLines(_orderId, ServiceAddress, "api/cartline/order");
            Assert.Equal(2, cartLines.Count);
            var cartRecord = cartLines[cartLines.Count - 1];
            Assert.Equal(2, cartRecord.Quantity);
        }

        //ShoppingCart08
        [Fact]
        public async void ShouldNotUpdateCartLineIfThereIsNotEnoughStock()
        {
            // https://localhost:5001/api/cartline/{orderId} HTTPPost
            // Initial data: {"ProductId":2,"Quantity":4,"OrderId":1} NumberOfRecords: 2
            using (var client = new HttpClient())
            {
                var productId = _productId;
                var quantity = 1;
                var targetUrl = $"{ServiceAddress}{RootAddressCartLineController}/{_orderId}";
                var response = await client.PostAsync(targetUrl,
                    new StringContent("{" + $"\"ProductId\":2,\"Quantity\":{quantity},\"OrderId\":{_orderId}" + "}",
                        Encoding.UTF8, "application/json"));
                Assert.False(response.IsSuccessStatusCode, response.ReasonPhrase);
                Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            }
        }

        //ShoppingCart09
        [Fact]
        public async void ShouldRemoveRecordOnAddIfQuantityBecomesLessThanZero()
        {
            // https://localhost:5001/api/cartline/{orderId} HTTPPost
            // Initial data: {"ProductId":2,"Quantity":4,"OrderId":1} NumberOfRecords: 2
            using (var client = new HttpClient())
            {
                var productId = 2;
                var quantity = -5;
                var targetUrl = $"{ServiceAddress}{RootAddressCartLineController}/{_orderId}";
                var response = await client.PostAsync(targetUrl,
                    new StringContent(
                        "{" + $"\"ProductId\":{productId},\"Quantity\":{quantity},\"OrderId\":{_orderId}" + "}",
                        Encoding.UTF8, "application/json"));
                Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
            }
            // validate the cart line was removed
            List<CartLine> cartLines =
                await CartLineTestHelpers.GetCartLines(_orderId, ServiceAddress, "api/cartline/order");
            Assert.Single(cartLines);
        }
    }
}

