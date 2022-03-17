using Xamarin.Forms;
using Data.Context;
using Xamarin.Essentials;

namespace XamarinApp
{
    public partial class App : Application
    {

        public App()
        {
            DependencyService.Register<AppDbContext>();
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
