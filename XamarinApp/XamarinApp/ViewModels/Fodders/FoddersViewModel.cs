using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Data.Context;
using Data.Models;
using XamarinApp.Views.Fodders;
using XamarinApp.ViewModels.Base;

namespace XamarinApp.ViewModels.Fodders
{
    class FoddersViewModel : BaseViewModel
    {
        public AppDbContext Db => DependencyService.Get<AppDbContext>();

        private Fodder _selectedItem;

        public ObservableCollection<Fodder> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Fodder> ItemTapped { get; }

        public FoddersViewModel()
        {
            Title = "Корма";
            Items = new ObservableCollection<Fodder>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Fodder>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await Db.Fodders
                    .ToListAsync();
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

        public Fodder SelectedItem
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
            await Shell.Current.GoToAsync(nameof(NewFodderPage));
        }

        async void OnItemSelected(Fodder item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(FodderDetailPage)}?{nameof(FodderDetailViewModel.ItemId)}={item.Id}");
        }
    }
}