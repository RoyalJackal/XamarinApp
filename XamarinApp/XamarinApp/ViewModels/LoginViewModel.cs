using Xamarin.Forms;
using XamarinApp.Services;
using XamarinApp.ViewModels.Base;
using XamarinApp.Views.Auth;
using XamarinApp.Views.Pets;

namespace XamarinApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public AuthService Auth => DependencyService.Get<AuthService>();
        public AlertService Alerts => DependencyService.Get<AlertService>();


        public Command LoginCommand { get; }

        private string username;

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        private string password;

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            IsBusy = true;
            var result = await Auth.SignIn(Username, Password);
            if (result)
            {
                var shell = (AppShell)Shell.Current;
                var shellViewModel = (AppShellViewModel)shell.BindingContext;
                shellViewModel.IsAuthenticated = true;
                IsBusy = false;
                await Shell.Current.GoToAsync($"//{nameof(PetsPage)}");
            }
            else
            {
                IsBusy = false;
                await Alerts.ShowErrorAsync("Ошибка при входе.");
            }
        }
    }
}
