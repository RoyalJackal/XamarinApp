using System;
using Xamarin.Forms;
using Data.Context;
using Plugin.FirebasePushNotification;
using Plugin.LocalNotification;
using Xamarin.Essentials;
using XamarinApp.Settings;
using XamarinApp.Services;

namespace XamarinApp
{
    public partial class App : Application
    {
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
            DependencyService.Register<NotificationService>();
            DependencyService.RegisterSingleton(new AudioService());
            InitializeComponent();

            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            var uri = Preferences.Get("lastPage", Shell.Current.CurrentState.Location.ToString());
            if (uri != null)
                await Shell.Current.GoToAsync(uri);
            
            CrossFirebasePushNotification.Current.OnTokenRefresh += OnTokenRefresh;
        }
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public async void OnTokenRefresh(object source, FirebasePushNotificationTokenEventArgs e)
        {
            await DependencyService.Get<AuthService>().EnsureDeviceTokenUpdated(e.Token);
        }

        public async void OnNotificationReceived(object source, FirebasePushNotificationDataEventArgs e)
        {
            if (!e.Data.TryGetValue("title", out var title) || !e.Data.TryGetValue("body", out var body))
                return;

            await NotificationCenter.Current.Show(notification => notification
                .WithTitle(title as string)
                .WithDescription(body as string)
                .WithNotificationId(new Random().Next())
                .Create());
        }
    }
}
