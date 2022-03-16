using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.ViewModels.Pets;

namespace XamarinApp.Views.Pets
{
    public partial class NewPetPage : ContentPage
    {
        public Pet Item { get; set; }

        public NewPetPage()
        {
            InitializeComponent();
            BindingContext = new NewPetViewModel();
        }
    }
}