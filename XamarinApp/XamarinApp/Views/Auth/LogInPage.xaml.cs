using XamarinApp.ViewModels;
using XamarinApp.Views.Base;

namespace XamarinApp.Views.Auth
{
    public partial class LogInPage : BasePage
    {
        public LogInPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }
    }
}