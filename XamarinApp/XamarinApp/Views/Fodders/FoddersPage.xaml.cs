using Xamarin.Forms;
using XamarinApp.ViewModels.Fodders;

namespace XamarinApp.Views.Fodders
{
    public partial class FoddersPage : ContentPage
    {
        FoddersViewModel _viewModel;

        public FoddersPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new FoddersViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}