using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApp.Settings
{
    class ApiEndpoints
    {
        private readonly string _baseUrl;

        public ApiEndpoints()
        {
            _baseUrl = "https://localhost:44382";
            //_baseUrl = "https://192.168.0.101:5000";
        }

        public string AuthSignUp => $"{_baseUrl}/Auth/SignUp";
        public string AuthSignIn => $"{_baseUrl}/Auth/SignIn";
    }
}
