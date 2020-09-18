using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BE.Service.Tests.Helpers
{
    public class AccountTestHelpers
    {
        public static async Task LoginAsUser(string serviceAddress, string rootAddress, HttpClient client)
        { 
            var targetLogin = $"{serviceAddress}{rootAddress}";
            var responseLogIn = await client.PostAsync(targetLogin,
                new StringContent("{\"name\":\"user\",\"password\":\"password\"}", Encoding.UTF8, "application/json"));
            Assert.True(responseLogIn.IsSuccessStatusCode, responseLogIn.ReasonPhrase);
        }
        public static async Task LoginAsAdmin(string serviceAddress, string rootAddress, HttpClient client)
        {
            var targetLogin = $"{serviceAddress}{rootAddress}";
            var responseLogIn = await client.PostAsync(targetLogin,
                new StringContent("{\"name\":\"admin\",\"password\":\"secret\"}", Encoding.UTF8, "application/json"));
            Assert.True(responseLogIn.IsSuccessStatusCode, responseLogIn.ReasonPhrase);
        }
        public static async Task Logout(string serviceAddress, string rootAddress, HttpClient client)
        {
                var targetLogout = $"{serviceAddress}api/account/logout";
                var responseLogOut = await client.PostAsync(targetLogout, null);
                Assert.True(responseLogOut.IsSuccessStatusCode, responseLogOut.ReasonPhrase);
        }
    }
}
