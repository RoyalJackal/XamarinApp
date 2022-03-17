using Xamarin.Forms;
using XamarinApp.ViewModels.Feeds;
using XamarinApp.Views.Base;

namespace XamarinApp.Views.Feeds
{
    public partial class FeedDetailPage : BasePage
    {
        public FeedDetailPage()
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            InitializeComponent();
            BindingContext = new FeedDetailViewModel();
        }
    }
}