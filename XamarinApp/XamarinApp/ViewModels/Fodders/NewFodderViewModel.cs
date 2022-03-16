using System;
using System.Diagnostics;
using Xamarin.Forms;
using XamarinApp.Models;

namespace XamarinApp.ViewModels.Fodders
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class NewFodderViewModel : BaseViewModel
    {
        private string name;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private int itemId;
        public int ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                if (itemId != default)
                    LoadItemId(value);
            }
        }

        public async void LoadItemId(int itemId)
        {
            var item = await App.Db.Table<Fodder>()
                .Where(i => i.Id == itemId)
                .FirstOrDefaultAsync();

            if (item == null)
                Debug.WriteLine("Failed to Load Item");
            else
            {
                Name = item.Name;
            }
        }

        public NewFodderViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave() =>
            !String.IsNullOrWhiteSpace(name);


        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var newItem = new Fodder()
            {
                Id = ItemId,
                Name = Name
            };

            if (newItem.Id != 0)
                await App.Db.UpdateAsync(newItem);
            else
                await App.Db.InsertAsync(newItem);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
