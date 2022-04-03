using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using XamarinApp.Services;

namespace XamarinApp.Helpers
{
    public static class HttpClientHelper
    {
        public static HttpClient GetClient()
        {
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
                (message, certificate, chain, sslPolicyErrors) => true;

            return new HttpClient(httpClientHandler);
        }
        
        public static async Task<HttpClient> GetAuthenticatedClient()
        {
            var httpClientHandler = new HttpClientHandler();

            httpClientHandler.ServerCertificateCustomValidationCallback =
                (message, certificate, chain, sslPolicyErrors) => true;

            var token = await AuthService.GetToken();

            var client = new HttpClient(httpClientHandler);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}
