using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace XamarinApp.Settings
{
    public class ApiEndpoints
    {
        private readonly string _baseUrl;

        public ApiEndpoints()
        {
            _baseUrl = "http://192.168.1.60:5000";
            //_baseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "https://10.0.2.2:5000" : "https://localhost:5000";
        }

        public string AuthSignUp => $"{_baseUrl}/Auth/SignUp";
        public string AuthSignIn => $"{_baseUrl}/Auth/SignIn";
        
        public string NotifierGetNotifiers => $"{_baseUrl}/Notification/GetNotifications";
        public string NotifierAddNotifier => $"{_baseUrl}/Notification/AddNotification";
        public string NotifierAddToken => $"{_baseUrl}/Notification/AddToken";
        public string NotifierRemoveToken => $"{_baseUrl}/Notification/RemoveToken";
    }
}
