using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BE.Models.Entities;
using Newtonsoft.Json;
using Xunit;

namespace BE.Service.Tests.Helpers
{
    public class AdminTestHelpers
    {
        public static async Task<AppUser> GetUser(string Id, string serviceAddress, string rootAddress, HttpClient client)
        {
            var response = await client.GetAsync($"{serviceAddress}{rootAddress}/{Id}");
            Assert.True(response.IsSuccessStatusCode);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<AppUser>(jsonResponse);
        }
    }
}
