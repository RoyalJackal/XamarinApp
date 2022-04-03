using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace XamarinApp.Settings
{
    class ApiEndpoints
    {
        private readonly string _baseUrl;

        public ApiEndpoints()
        {
            _baseUrl = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "https://localhost:5000";
        }

        public string AuthSignUp => $"{_baseUrl}/Auth/SignUp";
        public string AuthSignIn => $"{_baseUrl}/Auth/SignIn";
    }
}
