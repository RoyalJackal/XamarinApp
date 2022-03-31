using System;
using System.Diagnostics;
using Xamarin.Forms;
using Data.Context;
using Data.Models;
using XamarinApp.ViewModels.Base;

namespace XamarinApp.ViewModels.Pets
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    class NewPetViewModel : BaseViewModel
    {
        public AppDbContext Db => DependencyService.Get<AppDbContext>();

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
            var item = await Db.Pets.FindAsync(itemId);

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
            if (ItemId == default)
            {
                var pet = new Pet()
                {
                    Name = Name,
                    Breed = Breed
                };
                await Db.AddAsync(pet);
            }
            else
            {
                var pet = await Db.Pets.FindAsync(ItemId);
                pet.Name = Name;
                pet.Breed = Breed;
            }
            await Db.SaveChangesAsync();

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
