using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using XamarinApp.Services;
using XamarinApp.ViewModels.Base;
using XamarinApp.Views.Auth;

namespace XamarinApp.ViewModels
{
    public class AppShellViewModel : BaseViewModel
    {
        public AuthService Auth => DependencyService.Get<AuthService>();

        public Command GoToLoginCommand { get; }
        public Command GoToSignInCommand { get; }
        public Command LogoutCommand { get; }


        private bool isAuthenticated;
        public bool IsAuthenticated
        {
            get => isAuthenticated;
            set => SetProperty(ref isAuthenticated, value);
        }

        public bool IsNotAuthenticated
        {
            get => !isAuthenticated;
        }

        public AppShellViewModel()
        {
            var authTask = Auth.IsAuthenticated();
            //authTask.Wait();
            //IsAuthenticated = authTask.Result;
            IsAuthenticated = false;

            GoToLoginCommand = new Command(OnLoginClicked);
            GoToSignInCommand = new Command(OnSignInClicked);
            LogoutCommand = new Command(OnLogoutClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(LogInPage)}");
        }

        private async void OnSignInClicked(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(SignInPage)}");
        }

        private async void OnLogoutClicked(object obj)
        {
            Auth.Exit();
            IsAuthenticated = false;
            await Shell.Current.GoToAsync($"{nameof(LogInPage)}");
        }
    }
}
