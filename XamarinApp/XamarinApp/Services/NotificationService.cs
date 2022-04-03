using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shared.Dto;
using Xamarin.Forms;
using XamarinApp.Helpers;
using XamarinApp.Settings;

namespace XamarinApp.Services
{
    public class NotificationService
    {
        private readonly ApiEndpoints _endpoints = DependencyService.Get<ApiEndpoints>();
        public AlertService Alerts => DependencyService.Get<AlertService>();

        public async Task<List<NotificationDto>> GetNotifications()
        {
            try
            {
                var httpClient = await HttpClientHelper.GetAuthenticatedClient();
                var response = await httpClient.GetFromJsonAsync<List<NotificationDto>>(_endpoints.NotifierGetNotifiers);
                return response;
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }

        public async Task<bool> AddNotification(NotificationDto dto)
        {
            try
            {
                var httpClient = await HttpClientHelper.GetAuthenticatedClient();
                var response = await httpClient.PostAsJsonAsync(_endpoints.NotifierAddNotifier, dto);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await Alerts.ShowErrorAsync(ex.Message);
            }

            return false;
        }

        public async Task<bool> AddToken(string token)
        {
            try
            {
                var httpClient = await HttpClientHelper.GetAuthenticatedClient();
                var response = await httpClient.PostAsJsonAsync(_endpoints.NotifierAddToken, new FirebaseTokenDto{Token = token});

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await Alerts.ShowErrorAsync(ex.Message);
            }

            return false;
        }

        public async Task<bool> RemoveToken(string token)
        {
            try
            {
                var httpClient = await HttpClientHelper.GetAuthenticatedClient();
                var response = await httpClient.PostAsJsonAsync(_endpoints.NotifierRemoveToken,
                    new FirebaseTokenDto { Token = token });

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await Alerts.ShowErrorAsync(ex.Message);
            }

            return false;
        }
    }
}