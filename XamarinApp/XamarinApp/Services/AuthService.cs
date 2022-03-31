using Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using XamarinApp.Helpers;
using System.Net.Http;
using System.Net.Http.Json;
using XamarinApp.Settings;
using Xamarin.Forms;

namespace XamarinApp.Services
{
    public class AuthService
    {
        private const string AuthUsername = "AuthUsername";
        private const string AuthToken = "AuthToken";
        private const string AuthTokenExpiration = "AuthTokenExpiration";

        private readonly ApiEndpoints _endpoints;

        public AuthService()
        {
            _endpoints = DependencyService.Get<ApiEndpoints>();
        }

        public async Task<string> GetToken() => await SecureStorage.GetAsync(AuthToken);

        public async Task<DateTimeOffset> GetTokenExpiration() =>
            DateTimeOffset.Parse(await SecureStorage.GetAsync(AuthTokenExpiration));

        public async Task<bool> IsAuthenticated() =>
            await GetToken() != null && DateTimeOffset.Now < await GetTokenExpiration();

        public async Task<string> GetUsername() => await SecureStorage.GetAsync(AuthUsername);

        public async Task<bool> SignIn(string username, string password)
        {
            try
            {
                var httpClient = HttpClientHelper.GetClient();
                var response = await httpClient.PostAsJsonAsync(_endpoints.AuthSignIn, new SignInDto
                {
                    Username = username,
                    Password = password
                });

                if (!response.IsSuccessStatusCode) return false;

                var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();
                if (tokenDto == null) return false;

                await SecureStorage.SetAsync(AuthToken, tokenDto.Token);
                await SecureStorage.SetAsync(AuthTokenExpiration, tokenDto.Expiration.ToString());
                await SecureStorage.SetAsync(AuthUsername, tokenDto.UserName);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SignUp(string email, string userName, string password)
        {
            try
            {
                var httpClient = HttpClientHelper.GetClient();
                var response = await httpClient.PostAsJsonAsync(_endpoints.AuthSignUp, new SignUpDto
                {
                    Email = email,
                    Username = userName,
                    Password = password
                });

                if (!response.IsSuccessStatusCode) return false;

                var tokenDto = await response.Content.ReadFromJsonAsync<TokenDto>();
                if (tokenDto == null) return false;

                await SecureStorage.SetAsync(AuthToken, tokenDto.Token);
                await SecureStorage.SetAsync(AuthUsername, tokenDto.UserName);
                await SecureStorage.SetAsync(AuthTokenExpiration, tokenDto.Expiration.ToString());

                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Exit()
        {
            SecureStorage.Remove(AuthToken);
            SecureStorage.Remove(AuthUsername);
            SecureStorage.Remove(AuthTokenExpiration);
        }
    }
}
