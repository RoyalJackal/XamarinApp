using System;
using Xamarin.Forms;
using XamarinApp.Views.Feeds;
using XamarinApp.Views.Fodders;
using XamarinApp.Views.Pets;

namespace XamarinApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(FodderDetailPage), typeof(FodderDetailPage));
            Routing.RegisterRoute(nameof(NewFodderPage), typeof(NewFodderPage));
            Routing.RegisterRoute(nameof(PetDetailPage), typeof(PetDetailPage));
            Routing.RegisterRoute(nameof(NewPetPage), typeof(NewPetPage));
            Routing.RegisterRoute(nameof(FeedDetailPage), typeof(FeedDetailPage));
            Routing.RegisterRoute(nameof(NewFeedPage), typeof(NewFeedPage));

        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Current.GoToAsync("//LoginPage");
        }
    }
}
