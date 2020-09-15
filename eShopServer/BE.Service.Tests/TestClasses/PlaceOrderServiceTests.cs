﻿using System;
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
    public class PlaceOrderServiceTests : BaseTestClass
    {
        private readonly string RootAddressOrderController = String.Empty;

        public PlaceOrderServiceTests()
        {
            RootAddressOrderController = "api/orders";
            SampleDataInitializer.InitializeData(new BEIdentityContextFactory().CreateDbContext(null));
        }

        //PlaceOrder01
        [Fact]
        public async void ShouldPlaceOrderWithValidData()
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

        //PlaceOrder03
        [Fact]
        public async void ShouldNotPlaceOrderWhenIsMissingData()
        {
            using (var client = new HttpClient())
            {
                await AccountTestHelpers.LoginAsUser(ServiceAddress, $"api/account/login", client);
                object data = new Order
                {
                    Name = "NameCustomer1",
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
                var targetUrl = $"{ServiceAddress}{RootAddressOrderController}";
                var response = await client.PostAsync(targetUrl, new StringContent(myContent, Encoding.UTF8, "application/json"));
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                await AccountTestHelpers.Logout(ServiceAddress, $"api/account/logout", client);
            }
        }
    }
}


