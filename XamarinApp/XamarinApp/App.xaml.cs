using System;
using Xamarin.Forms;
using Data.Context;
using Plugin.FirebasePushNotification;
using Xamarin.Essentials;
using XamarinApp.Settings;
using XamarinApp.Services;

namespace XamarinApp
{
    public partial class App : Application
    {
        public string FirebaseToken { get; set; }
        public static double ScreenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        public static double ScreenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        public static double QuarterScreenHeight = ScreenHeight / 5;

        public App()
        {
            DependencyService.Register<AppDbContext>();
            DependencyService.Register<ApiEndpoints>();
            DependencyService.Register<AuthService>();
            DependencyService.Register<AlertService>();
            DependencyService.Register<MediaService>();
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            var uri = Preferences.Get("lastPage", Shell.Current.CurrentState.Location.ToString());
            if (uri != null)
                await Shell.Current.GoToAsync(uri);
            
            FirebaseToken = CrossFirebasePushNotification.Current.Token;
            CrossFirebasePushNotification.Current.OnTokenRefresh += OnTokenRefresh;
        }
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        
        private void OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            FirebaseToken = e.Token;
        }
    }
}
