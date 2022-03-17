using Xamarin.Forms;
using XamarinApp.ViewModels.Fodders;
using XamarinApp.Views.Base;

namespace XamarinApp.Views.Fodders
{
    public partial class FodderDetailPage : BasePage
    {
        public FodderDetailPage()
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            InitializeComponent();
            BindingContext = new FodderDetailViewModel();
        }
    }
}