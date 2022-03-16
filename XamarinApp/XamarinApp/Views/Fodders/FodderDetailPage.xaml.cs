using Xamarin.Forms;
using XamarinApp.ViewModels.Fodders;

namespace XamarinApp.Views.Fodders
{
    public partial class FodderDetailPage : ContentPage
    {
        public FodderDetailPage()
        {
            InitializeComponent();
            BindingContext = new FodderDetailViewModel();
        }
    }
}