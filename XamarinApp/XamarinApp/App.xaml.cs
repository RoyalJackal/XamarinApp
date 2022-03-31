using Xamarin.Forms;
using Data.Context;
using Xamarin.Essentials;
using XamarinApp.Settings;
using XamarinApp.Services;

namespace XamarinApp
{
    public partial class App : Application
    {

        public App()
        {
            DependencyService.Register<AppDbContext>();
            DependencyService.Register<ApiEndpoints>();
            DependencyService.Register<AuthService>();
            DependencyService.Register<AlertService>();
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected async override void OnStart()
        {
            var uri = Preferences.Get("lastPage", Shell.Current.CurrentState.Location.ToString());
            if (uri != null)
                await Shell.Current.GoToAsync(uri);
        }
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
