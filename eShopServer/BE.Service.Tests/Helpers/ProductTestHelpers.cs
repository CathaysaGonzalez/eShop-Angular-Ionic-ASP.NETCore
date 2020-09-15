//using AutoMapper;
using Newtonsoft.Json;
using BE.Models.Entities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BE.Service.Tests.Helpers
{
    public static class ProductTestHelpers
    {
        public static async Task<List<Product>> GetProducts(string serviceAddress, string rootAddress)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Product>>(jsonResponse);
            }
        }

        public static async Task<Product> GetProduct(long Id, string serviceAddress, string rootAddress)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{Id}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Product>(jsonResponse);
            }
        }
    }
}
