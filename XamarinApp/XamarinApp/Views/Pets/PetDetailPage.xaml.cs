using Xamarin.Forms;
using XamarinApp.ViewModels.Pets;
using XamarinApp.Views.Base;

namespace XamarinApp.Views.Pets
{
    public partial class PetDetailPage : BasePage
    {
        PetDetailViewModel _viewModel;

        public PetDetailPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new PetDetailViewModel();
        }

        protected override void OnAppearing()
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}