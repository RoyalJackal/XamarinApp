using System;
using Android;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Support.V4.Content;
using AndroidX.Core.App;
using Firebase;
using Firebase.Iid;
using Firebase.Messaging;
using Plugin.FirebasePushNotification;

namespace XamarinApp.Droid
{
    [Activity(Label = "XamarinApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public string Token { get; set; }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            AddPermissions();
            SetUpPush();
        }

        private void SetUpPush()
        {
            //Set the default notification channel for your app when running Android Oreo
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
            {
                //Change for your default notification channel id here
                FirebasePushNotificationManager.DefaultNotificationChannelId = "FirebasePushNotificationChannel";

                //Change for your default notification channel name here
                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
            }


            //If debug you should reset the token each time.
#if DEBUG
            FirebasePushNotificationManager.Initialize(this, true);
#else
            FirebasePushNotificationManager.Initialize(this,false);
#endif

            var app = (App)Xamarin.Forms.Application.Current;
            CrossFirebasePushNotification.Current.OnTokenRefresh += app.OnTokenRefresh;
            CrossFirebasePushNotification.Current.OnNotificationReceived += app.OnNotificationReceived;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void AddPermissions()
        {
            if (ContextCompat.CheckSelfPermission (this, Manifest.Permission.RecordAudio) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions (this, new String [] { Manifest.Permission.RecordAudio }, 1);
            }
            if (ContextCompat.CheckSelfPermission (this, Manifest.Permission.ManageExternalStorage) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions (this, new String [] { Manifest.Permission.ManageExternalStorage }, 1);
            }
            if (ContextCompat.CheckSelfPermission (this, Manifest.Permission.ReadExternalStorage) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions (this, new String [] { Manifest.Permission.ReadExternalStorage }, 1);
            }
            if (ContextCompat.CheckSelfPermission (this, Manifest.Permission.WriteExternalStorage) != Permission.Granted)
            {
                ActivityCompat.RequestPermissions (this, new String [] { Manifest.Permission.WriteExternalStorage }, 1);
            }
        }
    }
}