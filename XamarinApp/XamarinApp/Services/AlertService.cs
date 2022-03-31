using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamarinApp.Services
{
    public class AlertService
    {
        public async Task ShowErrorAsync(string message)
        {
            await App.Current.MainPage.DisplayAlert("Ошибка", message, "Принять");
        }
    }
}
