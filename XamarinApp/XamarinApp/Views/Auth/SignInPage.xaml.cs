using XamarinApp.ViewModels;

namespace XamarinApp.Views.Auth
{
    public partial class SignInPage
    {
        public SignInPage()
        {
            InitializeComponent();
            BindingContext = new SignInViewModel();
        }
    }
}