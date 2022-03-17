using Xamarin.Forms;
using XamarinApp.ViewModels.Fodders;
using XamarinApp.Views.Base;

namespace XamarinApp.Views.Fodders
{
    public partial class FoddersPage : BasePage
    {
        FoddersViewModel _viewModel;

        public FoddersPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new FoddersViewModel();
        }

        protected override void OnAppearing()
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}