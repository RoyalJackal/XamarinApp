using Xamarin.Forms;
using XamarinApp.ViewModels.Fodders;

namespace XamarinApp.Views.Fodders
{
    public partial class FodderDetailPage : ContentPage
    {
        public FodderDetailPage()
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            InitializeComponent();
            BindingContext = new FodderDetailViewModel();
        }
    }
}