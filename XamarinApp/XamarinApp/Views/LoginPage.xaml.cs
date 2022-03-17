using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinApp.ViewModels;
using XamarinApp.Views.Base;

namespace XamarinApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : BasePage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }
    }
}