using Xamarin.Forms;
using Data.Models;
using XamarinApp.ViewModels.Fodders;
using XamarinApp.Views.Base;

namespace XamarinApp.Views.Fodders
{
    public partial class NewFodderPage : BasePage
    {
        public Fodder Item { get; set; }

        public NewFodderPage()
        {
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Disabled;
            InitializeComponent();
            BindingContext = new NewFodderViewModel();
        }
    }
}