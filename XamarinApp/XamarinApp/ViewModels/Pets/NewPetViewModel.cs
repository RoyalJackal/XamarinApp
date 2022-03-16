using System.Diagnostics;
using Xamarin.Forms;
using XamarinApp.Models;

namespace XamarinApp.ViewModels.Pets
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class NewPetViewModel : BaseViewModel
    {
        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string breed;
        public string Breed
        {
            get => breed;
            set => SetProperty(ref breed, value);
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

        public NewPetViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public async void LoadItemId(int itemId)
        {
            var item = await App.Db.Table<Pet>()
                .Where(i => i.Id == itemId)
                .FirstOrDefaultAsync();

            if (item == null)
                Debug.WriteLine("Failed to Load Item");
            else
            {
                Name = item.Name;
                Breed = item.Breed;
            }
        }

        private bool ValidateSave() =>
            !string.IsNullOrWhiteSpace(name);

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var newItem = new Pet()
            {
                Id = ItemId,
                Name = Name,
                Breed = Breed
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
