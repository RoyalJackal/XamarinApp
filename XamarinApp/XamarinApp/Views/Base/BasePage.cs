using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinApp.Views.Base
{
    public abstract class BasePage : ContentPage
    {
        public BasePage()
        {
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Preferences.Set("lastPage", Shell.Current.CurrentState.Location.ToString());
        }
    }
}
