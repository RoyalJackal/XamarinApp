using Xamarin.Forms;
using XamarinApp.ViewModels.Feeds;

namespace XamarinApp.Views.Feeds
{
    public partial class FeedDetailPage : ContentPage
    {
        public FeedDetailPage()
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            InitializeComponent();
            BindingContext = new FeedDetailViewModel();
        }
    }
}