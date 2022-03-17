using Xamarin.Forms;
using Data.Models;
using XamarinApp.ViewModels.Fodders;

namespace XamarinApp.Views.Fodders
{
    public partial class NewFodderPage : ContentPage
    {
        public Fodder Item { get; set; }

        public NewFodderPage()
        {
            InitializeComponent();
            BindingContext = new NewFodderViewModel();
        }
    }
}