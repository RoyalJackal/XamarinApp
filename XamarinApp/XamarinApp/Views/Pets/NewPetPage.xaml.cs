using Xamarin.Forms;
using Data.Models;
using XamarinApp.ViewModels.Pets;
using XamarinApp.Views.Base;

namespace XamarinApp.Views.Pets
{
    public partial class NewPetPage : BasePage
    {
        public Pet Item { get; set; }

        public NewPetPage()
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            InitializeComponent();
            BindingContext = new NewPetViewModel();
        }
    }
}