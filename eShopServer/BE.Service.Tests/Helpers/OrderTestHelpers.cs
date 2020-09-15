using BE.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BE.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace BE.Service.Tests.Helpers
{
    public static class OrderTestHelpers
    {
        public static async Task CreateOrder(string serviceAddress, string rootAddress, HttpClient client)
        {
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
            var targetUrl = $"{serviceAddress}api/orders";
            var response = await client.PostAsync(targetUrl,
                new StringContent(myContent, Encoding.UTF8, "application/json"));
            Assert.True(response.IsSuccessStatusCode, response.ReasonPhrase);
        }
    

        public static async Task Create3OrdersForCurrentUser(string serviceAddress, string rootAddress, HttpClient client)
        {
            await CreateOrder(serviceAddress, rootAddress, client);
            await CreateOrder(serviceAddress, rootAddress, client);
            await CreateOrder(serviceAddress, rootAddress, client);
        }

        public static async Task<List<Order>> GetOrders(string serviceAddress, string rootAddress, HttpClient client)
        { 
            var response = await client.GetAsync($"{serviceAddress}api/orders/test");
            Assert.True(response.IsSuccessStatusCode);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Order>>(jsonResponse);
        }

        public static async Task<Order> GetOrder(long Id, string serviceAddress, string rootAddress, HttpClient client)
        {
            var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{Id}");
            Assert.True(response.IsSuccessStatusCode);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Order>(jsonResponse);
        }
    }
}
