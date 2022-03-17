using Xamarin.Forms;
using Data.Context;

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

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
