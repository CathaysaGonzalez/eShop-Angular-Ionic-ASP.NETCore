using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BE.Models.Entities;
using Newtonsoft.Json;
using Xunit;

namespace BE.Service.Tests.Helpers
{
    public static class CartLineTestHelpers
    {
        public static async Task<CartLine> GetCartLine(long orderId, string serviceAddress, string rootAddress)
        {
            using (var client = new HttpClient())
            {
                var response =
                    await client.GetAsync($"{serviceAddress}{rootAddress}/{orderId}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<CartLine>(jsonResponse);
            }
        }

        public static async Task<List<CartLine>> GetCartLines(long orderId, string serviceAddress, string rootAddress)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{orderId}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<CartLine>>(jsonResponse);
            }
        }

        public static async Task<Order> GetOrder(long orderId, string serviceAddress, string rootAddress)
        {
            using (var client = new HttpClient())
            {
                var response =
                    await client.GetAsync($"{serviceAddress}{rootAddress}/test/{orderId}");
                //Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Order>(jsonResponse);
            }
        }
    }
}
