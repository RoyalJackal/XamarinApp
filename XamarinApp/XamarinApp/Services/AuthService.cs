using Shared.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using XamarinApp.Helpers;
using System.Net.Http;
using System.Net.Http.Json;
using Plugin.FirebasePushNotification;
using XamarinApp.Settings;
using Xamarin.Forms;

namespace XamarinApp.Services
{
    public class AuthService
    {
        public AlertService Alerts => DependencyService.Get<AlertService>();

        private const string AuthUsername = "AuthUsername";
        public const string AuthToken = "AuthToken";
        private const string AuthTokenExpiration = "AuthTokenExpiration";
        private const string FirebaseToken = "FirebaseToken";

        private readonly ApiEndpoints _endpoints = DependencyService.Get<ApiEndpoints>();
        private readonly NotificationService _notifications = DependencyService.Get<NotificationService>();

        public static async Task<string> GetToken() => await SecureStorage.GetAsync(AuthToken);

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
                
                await EnsureDeviceTokenUpdated();

                return true;
            }
            catch (Exception ex)
            {
                await Alerts.ShowErrorAsync(ex.Message);
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
                
                await EnsureDeviceTokenUpdated();

                return true;
            }
            catch (Exception ex)
            {
                await Alerts.ShowErrorAsync(ex.Message);
                return false;
            }
        }

        public async Task Exit()
        {
            await EnsureDeviceTokenRemoved();
            
            SecureStorage.Remove(AuthToken);
            SecureStorage.Remove(AuthUsername);
            SecureStorage.Remove(AuthTokenExpiration);
        }
        
        public async Task EnsureDeviceTokenUpdated(string newToken = null)
        {
            if(!await IsAuthenticated())
                return;
            await EnsureDeviceTokenRemoved();

            if (newToken == null)
                newToken = CrossFirebasePushNotification.Current.Token;

            await _notifications.AddToken(newToken);
            await SecureStorage.SetAsync(FirebaseToken, newToken);
        }
        public async Task EnsureDeviceTokenRemoved()
        {
            if(!await IsAuthenticated())
                return;
            var oldDeviceToken = await SecureStorage.GetAsync(FirebaseToken);
            if (oldDeviceToken != null)
            {
                await _notifications.RemoveToken(oldDeviceToken);
                SecureStorage.Remove(FirebaseToken);
            }
        }
    }
}
