using Xamarin.Forms;
using XamarinApp.Services;
using XamarinApp.ViewModels.Base;
using XamarinApp.Views.Pets;

namespace XamarinApp.ViewModels
{
    class SignInViewModel : BaseViewModel
    {
        public AuthService Auth => DependencyService.Get<AuthService>();
        public AlertService Alerts => DependencyService.Get<AlertService>();

        public Command SignInCommand { get; }

        private string username;

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }


        private string email;
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string password;

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public SignInViewModel()
        {
            SignInCommand = new Command(OnSignInClicked);
        }

        private async void OnSignInClicked(object obj)
        {
            IsBusy = true;
            var result = await Auth.SignUp(Email, Username, Password);
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
