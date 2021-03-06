using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Data.Context;
using Data.Models;
using XamarinApp.Views.Pets;
using XamarinApp.ViewModels.Base;

namespace XamarinApp.ViewModels.Pets
{
    class PetsViewModel : BaseViewModel
    {
        public AppDbContext Db => DependencyService.Get<AppDbContext>();

        private Pet _selectedItem;

        public ObservableCollection<Pet> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Pet> ItemTapped { get; }

        public PetsViewModel()
        {
            Title = "Питомцы";
            Items = new ObservableCollection<Pet>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Pet>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await Db.Pets.ToListAsync();
                foreach (var item in items)
                    Items.Add(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Pet SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewPetPage));
        }

        async void OnItemSelected(Pet item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(PetDetailPage)}?{nameof(PetDetailViewModel.ItemId)}={item.Id}");
        }
    }
}